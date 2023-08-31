using System.Collections.Generic;
using Newtonsoft.Json;
using JsonSubTypes;




public static class JSONManager
{
   

    public static class MMTURIs
    {
        public const string Point = "http://mathhub.info/MitM/core/geometry?3DGeometry?point";
        public const string Tuple = "http://gl.mathhub.info/MMT/LFX/Sigma?Symbols?Tuple";
        public const string LineType = "http://mathhub.info/MitM/core/geometry?Geometry/Common?line_type";
        public const string LineOf = "http://mathhub.info/MitM/core/geometry?Geometry/Common?lineOf";

        public const string OnLine = "http://mathhub.info/MitM/core/geometry?Geometry/Common?onLine";
        public const string Ded = "http://mathhub.info/MitM/Foundation?Logic?ded";
        public const string Eq = "http://mathhub.info/MitM/Foundation?Logic?eq";
        public const string Metric = "http://mathhub.info/MitM/core/geometry?Geometry/Common?metric";
        public const string Angle = "http://mathhub.info/MitM/core/geometry?Geometry/Common?angle_between";
        public const string Sketch = "http://mathhub.info/MitM/Foundation?InformalProofs?proofsketch";
        public const string RealLit = "http://mathhub.info/MitM/Foundation?RealLiterals?real_lit";

        public const string ParallelLine = "http://mathhub.info/MitM/core/geometry?Geometry/Common?parallelLine";
        // public string RectangleFact = "http://mathhub.info/FrameIT/frameworld?FrameITRectangles?rectangleType";
        //  public string RectangleFactmk = "http://mathhub.info/FrameIT/frameworld?FrameITRectangles?mkRectangle";

        public const string CircleType3d = "http://mathhub.info/FrameIT/frameworld?FrameITCircle?circleType3D";
        public const string MkCircle3d = "http://mathhub.info/FrameIT/frameworld?FrameITCircle?circle3D";
        public const string TriangleMiddlePoint = "http://mathhub.info/FrameIT/frameworld?FrameITTriangles?triangleMidPointWrapper";
        public const string RadiusCircleMetric = "http://mathhub.info/FrameIT/frameworld?FrameITCircle?circleRadius";

        public const string AreaCircle = "http://mathhub.info/FrameIT/frameworld?FrameITCircle?areaCircle";
        public const string VolumeCone = "http://mathhub.info/FrameIT/frameworld?FrameITCone?volumeCone";
        public const string ConeOfCircleApex = "http://mathhub.info/FrameIT/frameworld?FrameITCone?circleConeOf";

        public const string ParametrizedPlane = "http://mathhub.info/MitM/core/geometry?Planes?ParametrizedPlane";
        public const string pointNormalPlane = "http://mathhub.info/MitM/core/geometry?Planes?pointNormalPlane";
        public const string OnCircle = "http://mathhub.info/FrameIT/frameworld?FrameITCircle?pointOnCircle";
        public const string AnglePlaneLine = "http://mathhub.info/FrameIT/frameworld?FrameITCircle?angleCircleLine";
        public const string OrthoCircleLine = "http://mathhub.info/FrameIT/frameworld?FrameITCircle?orthogonalCircleLine";

        public const string TruncatedVolumeCone = "http://mathhub.info/FrameIT/frameworld?FrameITCone?truncatedConeVolume";
        public const string CylinderVolume = "http://mathhub.info/FrameIT/frameworld?FrameITCylinder?cylinderVolume";
        public const string EqualityCircles = "http://mathhub.info/FrameIT/frameworld?FrameITCircle?equalityCircles";
        public const string UnEqualityCircles = "http://mathhub.info/FrameIT/frameworld?FrameITCircle?unequalityCircles";

        public const string ParallelCircles = "http://mathhub.info/FrameIT/frameworld?FrameITCone?parallelCircles";
        public const string RightAngle = "http://mathhub.info/FrameIT/frameworld?FrameITBasics?rightAngle";

        public const string TestType = "http://mathhub.info/FrameIT/frameworld?FrameITCircle?xcircleType3D";

    }


    public class URI
    {
        public string uri;

        public URI(string uri)
        {
            this.uri = uri;
        }
    }
    
    [JsonConverter(typeof(JsonSubtypes), "kind")]
    public class MMTTerm
    {
        string kind;
    }

    public class OMA : MMTTerm
    {
        public MMTTerm applicant;
        public List<MMTTerm> arguments;
        public string kind = "OMA";
        public OMA(MMTTerm applicant, List<MMTTerm> arguments)
        {
            this.applicant = applicant;
            this.arguments = arguments;
        }

        
    }

    public class OMS : MMTTerm
    {
        public string uri;
        public string kind = "OMS";

        public OMS(string uri)
        {
            this.uri = uri;
        }
    }

    public class OMSTR : MMTTerm
    {
        [JsonProperty("float")]
        public string s;
        public string kind = "OMSTR";

        public OMSTR(string s)
        {
            this.s = s;
        }
    }

    public class OMF : MMTTerm
    {
        [JsonProperty("float")]
        public float f;
        public string kind = "OMF";

        public OMF(float f)
        {
            this.f = f;
        }
    }

    public class MMTDeclaration
    {
        public string label;
        public static MMTDeclaration FromJson(string json)
        {
            MMTDeclaration mmtDecl = JsonConvert.DeserializeObject<MMTDeclaration>(json);
            if (mmtDecl.label == null)
                mmtDecl.label = string.Empty;

            return mmtDecl;
        }
        public static string ToJson(MMTDeclaration mmtDecl)
        {
            if (mmtDecl.label == null)
                mmtDecl.label = string.Empty;

            string json = JsonConvert.SerializeObject(mmtDecl);
            return json;
        }
    }

    /**
     * MMTSymbolDeclaration: Class for facts without values, e.g. Points
     */ 
    public class MMTSymbolDeclaration : MMTDeclaration
    {
        public string kind = "general";
        public MMTTerm tp;
        public MMTTerm df;

        /**
         * Constructor used for sending new declarations to mmt
         */
        public MMTSymbolDeclaration(string label, MMTTerm tp, MMTTerm df)
        {
            this.label = label;
            this.tp = tp;
            this.df = df;
        }
    }

    /**
     * MMTValueDeclaration: Class for facts with values, e.g. Distances or Angles
     */
    public class MMTValueDeclaration : MMTDeclaration
    {
        public string kind = "veq";
        public MMTTerm lhs;
        public MMTTerm valueTp;
        public MMTTerm value;

        /**
         * Constructor used for sending new declarations to mmt
         */
        public MMTValueDeclaration(string label, MMTTerm lhs, MMTTerm valueTp, MMTTerm value)
        {
            this.label = label;
            this.lhs = lhs;
            this.valueTp = valueTp;
            this.value = value;
        }
    }
}
