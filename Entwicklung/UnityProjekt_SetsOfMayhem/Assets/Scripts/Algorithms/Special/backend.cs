
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// sadly, this does not compile because of primitive vs complexe data types
//using Card = uint[];
using Card = System.Collections.Generic.List<uint>;


public enum GameState : uint {
	Init,       // game not initialized yet, call init() first
	Ready,      // ready for user input, call step(user_input)
	Again,      // user input is not a valid SET, try again
	Won,        // game is won!
	Error,      // an error occured
}

public class Game {
	/* card properties: array of length N, each value 0 <= c[i] < M */
	private uint N; // features
	private uint M; // values per feature
	private uint defaultSize;
	private uint currentSize;
	private uint maximumSize;
	private uint cardsLeft;
	private Card[] field;
	private Card[] cards;
	private Card[] helpSET;
	private GameState state;

	public Game(uint n, uint m) {
		N = n;
		M = m;
		defaultSize = n*m;
		currentSize = 0;
		maximumSize = defaultSize*2;
		cardsLeft = m^n;
		uint[] field = new uint[maximumSize];
		uint[,] cards = new uint[cardsLeft,n];
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
		// generate cards
		for (uint depth = 0; depth < N; depth++) {
			uint c = 0;
			for (uint i = 0; i < cardsLeft; i++) {
				cards[i][(int)depth] = c;
				if ((i+1) % (M^depth) == 0) {
					c++;
				}
			}
		}
		// shuffle cards
		System.Random rand = new System.Random();
		int c2 = (int)cardsLeft;
		while (c2 > 1) {
			int k = rand.Next(c2--);
			Card tmp = cards[c2];
			cards[c2] = cards[k];
			cards[k] = tmp;
		}
	}

	public GameState init() {
		if (N < M) return GameState.Error;

		generateCards();
		// initially fill field
		for (uint i = 0; i < defaultSize; i++) {
			field[currentSize++] = cards[--cardsLeft];
		}
		return (fillField() ? GameState.Error : GameState.Ready);
	}

	public bool isSET(Card[] set) {
		if (set.Length != M) return false;

		uint[] feature = new uint[M];
		for (uint n = 0; n < N; n++) {
			for (uint m = 0; m < M; m++) {
				feature[m] = set[m][(int)n];
			}
			
			//feature.sort();
			Array.Sort(feature);
			bool equal = false;
			bool different = false;
			uint tmp = feature[0];
			for (uint m = 1; m < M; m++) {
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

	private Card[] findSEThelper(uint indexSET, uint indexField, Card[] set) {
		if (indexSET >= M) return (isSET(set) ? set : null);

		for (uint f = indexField; f < currentSize; f++) {
			set[indexSET] = field[f];
			Card[] maybeSET = findSEThelper(indexSET+1, f+1, set);
			if (maybeSET != null) {
				return maybeSET;
			}
		}
		return null;
	}

	private Card[] findSET() {
		//return findSEThelper(0, 0, new uint[M][]);
		Card[] nc = new Card[M];
		return findSEThelper(0, 0, nc);
	}

	// return true if game is won
	private bool fillField() {
		while (true) {
			//uint[,] SET = findSET();
			Card[] SET = findSET();
			if (SET != null) {
				helpSET = SET;
				return false;
			} else {
				if (cardsLeft > M) {
					for (uint m = 0; m < M; m++) {
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
	public GameState step(uint[] iSET) {
		if (iSET.Length != M) return GameState.Again;

		bool allEqual = true;
		uint tmp = iSET[0];
		Card[] set = new Card[M];
		for (uint m = 0; m < M; m++) {
			set[m] = field[iSET[m]];
			if (iSET[m] != tmp) allEqual = false;
			tmp = iSET[m];
		}
		if (allEqual) return GameState.Again;
		if (!isSET(set)) return GameState.Again;
		//iSET.sort();
		Array.Sort(iSET);
		//iSET.reverse();
		Array.Reverse(iSET);
		if (currentSize > defaultSize || cardsLeft < M) {
			//for(i in iSET {
			for (int i=0; i< iSET.Length; i++) {
					if (i != --currentSize) {
					field[i] = field[currentSize];
				}
			}
		} else {
			for (int i = 0; i < iSET.Length; i++)
			{
				field[i] = cards[--cardsLeft];
			}
		}
		
		return (fillField() ? GameState.Won : GameState.Ready);
	}

}
