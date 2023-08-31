using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using static CommunicationEvents;

public class GenerateDemoFiles
{
    static bool firstcall = true;
    public static void GenerateAll()
    {
        if (!firstcall) return;
        if (GameObject.FindObjectOfType<GadgetBehaviour>(true) == null) {
            Debug.LogError("Cannot GenerateDemoFiles without populated GadgetManager");
            return;
        }

        Debug.LogWarning("Generating and Overwriting Stage Files");
        firstcall = false;

        GenerateTreeStage();
        GenerateRiverStage();
    }

    public static void GenerateTreeStage()
    {
        // Params
        float minimalSolutionHight = 6;

        // Generate Stage
        // TODO? use constructor
        Stage demo = new Stage
        {
            number = 1,
            category = "Demo Category",
            name = "TechDemo A",
            scene = "RiverWorld",
            description = "Tree Stage",
            use_install_folder = true,
            //hierarchie = new List<Directories> { /*Directories.Stages*/ }
        };

        // needed to generate facts
        StageStatic.StageOfficial = new Dictionary<string, Stage>
        {
            { demo.name, demo },
        };
        StageStatic.SetStage(demo.name, false);

        // Populate Solution
        PointFact
            buttom = new PointFact(Vector3.zero, Vector3.up, StageStatic.stage.solution),
            top = new PointFact(Vector3.zero + Vector3.up * minimalSolutionHight, Vector3.up, StageStatic.stage.solution);

        StageStatic.stage.solution.Add(buttom, out _, false, null, null);
        StageStatic.stage.solution.Add(top, out _, true, null, null);

        LineFact target = new LineFact(buttom.Id, top.Id, StageStatic.stage.solution);
        var target_Id = StageStatic.stage.solution.Add(target, out _, true, null, null);

        // Set Solution
        StageStatic.stage.solution.ValidationSet =
            new List<SolutionOrganizer.SubSolution>
            { new SolutionOrganizer.SubSolution(new HashSet<string> { target_Id }, null, null, new LineFactHightDirectionComparer()) };

        // Set Gadgets/ Scrolls
        StageStatic.stage.AllowedGadgets = null;
        StageStatic.stage.AllowedScrolls = null;

        // Save
        StageStatic.SetMode(StageStatic.Mode.Create);
        StageStatic.stage.store(false, true);
    }

    public static void GenerateRiverStage()
    {
        // Params
        float minimalSolutionHight = 6;

        // Generate Stage
        // TODO? use constructor
        Stage demo = new Stage
        {
            number = 2,
            category = "Demo Category",
            name = "TechDemo B",
            scene = "RiverWorld",
            description = "River Stage",
            use_install_folder = true,
            //hierarchie = new List<Directories> { /*Directories.Stages*/ }
        };

        // needed to generate facts
        StageStatic.StageOfficial = new Dictionary<string, Stage>
        {
            { demo.name, demo },
        };
        StageStatic.SetStage(demo.name, false);

        // Populate Solution
        PointFact
            buttom = new PointFact(Vector3.zero, Vector3.up, StageStatic.stage.solution),
            top = new PointFact(Vector3.zero + Vector3.up * minimalSolutionHight, Vector3.up, StageStatic.stage.solution);

        StageStatic.stage.solution.Add(buttom, out _, false, null, null);
        StageStatic.stage.solution.Add(top, out _, true, null, null);

        LineFact target = new LineFact(buttom.Id, top.Id, StageStatic.stage.solution);
        var target_Id = StageStatic.stage.solution.Add(target, out _, true, null, null);

        // Set Solution
        StageStatic.stage.solution.ValidationSet =
            new List<SolutionOrganizer.SubSolution> {
                new SolutionOrganizer.SubSolution(new HashSet<string> { target_Id }, null, null, new LineFactHightDirectionComparer()),
                new SolutionOrganizer.SubSolution(new HashSet<string> { target_Id }, null, null, new LineSpanningOverRiverWorldComparer()),
                new SolutionOrganizer.SubSolution(null, new List<int> { 1 }, new List<int> { 0 }, new LineFactHightComparer()),
            };

        // Set Gadgets/ Scrolls
        StageStatic.stage.AllowedGadgets = new() { new Pointer(), new Tape(), new AngleTool(), new LineTool(), new LotTool(), new Pendulum(), new Remover() }; //, new EqualCircleGadget(), new TestMiddlePoint() };
        StageStatic.stage.AllowedScrolls = new() { "OppositeLen" };//, "AngleSum", "Pythagoras", "CircleScroll", "CircleAreaScroll", "ConeVolumeScroll", "TruncatedConeVolumeScroll", "CylinderVolumeScroll", "MidPoint", "CircleLineAngleScroll", "CircleLineAngleToAngle", "SupplementaryAngles" };

        // Save
        StageStatic.SetMode(StageStatic.Mode.Create);
        StageStatic.stage.store(false, true);
    }
}
