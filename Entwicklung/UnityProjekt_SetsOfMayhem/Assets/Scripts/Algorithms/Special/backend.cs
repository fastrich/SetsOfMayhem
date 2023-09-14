using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// sadly, this does not compile because of primitive vs complexe data types
//using Card = int[];
using Card = System.Collections.Generic.List<int>;

public enum GameState {
	Init,       // game not initialized yet, call init() first
	Ready,      // ready for user input, call step(user_input)
	Again,      // user input is not a valid SET, try again
	Won,        // game is won!
	Error,      // an error occured
}

public class Game {
	/* card properties: array of length N, each value 0 <= c[i] < M */
	private int N; // features
	private int M; // values per feature
	private int defaultSize;
	private int currentSize;
	private int maximumSize;
	private int cardsLeft;
	private Card[] field;
	private Card[] cards;
	private Card[] helpSET;
	private GameState state;

	public Game(int n, int m) {
		N = n;
		M = m;
		defaultSize = n*m;
		currentSize = 0;
		maximumSize = defaultSize*2;
		cardsLeft = m^n;
		int[] field = new int[maximumSize];
		int[,] cards = new int[cardsLeft,n];
		helpSET = null;
		state = GameState.Init;
	}

	public GameState getGameState() {
		return state;
	}

	public Card[] getHelp() {
		return helpSET;
	}

	private void generateCards() {
		int c;
		// generate cards
		for (int depth = 0; depth < N; depth++) {
			c = 0;
			for (int i = 0; i < cardsLeft; i++) {
				cards[i][depth] = c;
				if ((i+1) % (M^depth) == 0) {
					c++;
				}
			}
		}
		// shuffle cards
		System.Random rand = new System.Random();
		c = cardsLeft;
		while (c > 1) {
			int k = rand.Next(c--);
			Card tmp = cards[c];
			cards[c] = cards[k];
			cards[k] = tmp;
		}
	}

	public GameState init() {
		if (N < M || N < 1 || M < 1) return GameState.Error;

		generateCards();
		// initially fill field
		for (int i = 0; i < defaultSize; i++) {
			field[currentSize++] = cards[--cardsLeft];
		}
		return (fillField() ? GameState.Error : GameState.Ready);
	}

	public bool isSET(Card[] set) {
		if (set.Length != M) return false;

		int[] feature = new int[M];
		for (int n = 0; n < N; n++) {
			for (int m = 0; m < M; m++) {
				feature[m] = set[m][n];
			}
			
			Array.Sort(feature);
			bool equal = false;
			bool different = false;
			int tmp = feature[0];
			for (int m = 1; m < M; m++) {
				if (tmp == feature[m]) {
					equal = true;
				} else {
					tmp = feature[m];
					different = true;
				}
			}
			if (equal && different) {
				return false;
			}
		}
		return true;
	}

	private Card[] findSEThelper(int indexSET, int indexField, Card[] set) {
		if (indexSET >= M) return (isSET(set) ? set : null);

		for (int f = indexField; f < currentSize; f++) {
			set[indexSET] = field[f];
			Card[] maybeSET = findSEThelper(indexSET+1, f+1, set);
			if (maybeSET != null) {
				return maybeSET;
			}
		}
		return null;
	}

	private Card[] findSET() {
		Card[] set = new Card[M];
		return findSEThelper(0, 0, set);
	}

	// return true if game is won
	private bool fillField() {
		while (true) {
			Card[] SET = findSET();
			if (SET != null) {
				helpSET = SET;
				return false;
			} else {
				if (cardsLeft > M) {
					for (int m = 0; m < M; m++) {
						// TODO if currentSize >= maximumSize ...
						field[currentSize++] = cards[--cardsLeft];
					}
				} else {
					// all sets found!
					return true;
				}
			}
		}
	}

	// argument: user input indices of possible SET
	// return state after processing field
	public GameState step(int[] iSET) {
		if (iSET.Length != M) return GameState.Again;

		bool allEqual = true;
		int tmp = iSET[0];
		Card[] set = new Card[M];
		for (int m = 0; m < M; m++) {
			if (iSET[m] < 0 || iSET[m] >= currentSize) return GameState.Error;
			set[m] = field[iSET[m]];
			if (iSET[m] != tmp) allEqual = false;
			tmp = iSET[m];
		}
		if (allEqual) return GameState.Again;
		if (!isSET(set)) return GameState.Again;

		Array.Sort(iSET);
		Array.Reverse(iSET);
		if (currentSize > defaultSize || cardsLeft < M) {
			for (int i = 0; i < M; i++) {
					if (i != --currentSize) {
					field[i] = field[currentSize];
				}
			}
		} else {
			for (int i = 0; i < M; i++) {
				field[i] = cards[--cardsLeft];
			}
		}
		
		return (fillField() ? GameState.Won : GameState.Ready);
	}

}
