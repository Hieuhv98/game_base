#if UNITY_EDITOR && UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
 
 
public class BuildPostProcessorInfo
{
    [PostProcessBuild]
    public static void OnPostBuildProcessInfo(BuildTarget target, string pathXcode)
    {
        if (target == BuildTarget.iOS)
        {
            var infoPlistPath = pathXcode + "/Info.plist";
 
            PlistDocument document = new PlistDocument();
            document.ReadFromString(File.ReadAllText(infoPlistPath));
 
 
            PlistElementDict elementDict = document.root;
            
            elementDict.SetString("NSUserTrackingUsageDescription", "This identifier will be used to deliver personalized ads to you.");
            
            File.WriteAllText(infoPlistPath, document.WriteToString());
        }
    }

    [PostProcessBuild]
	public static void OnPostProcessBuildAddFirebaseFile(BuildTarget buildTarget, string pathToBuiltProject)
	{
		if (buildTarget == BuildTarget.iOS)
		{
			// Go get pbxproj file
			string projPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
			// PBXProject class represents a project build settings file,
			// here is how to read that in.
			PBXProject proj = new PBXProject();
			proj.ReadFromFile(projPath);
			// Copy plist from the project folder to the build folder
			proj.AddFileToBuild(proj.GetUnityMainTargetGuid(), proj.AddFile("GoogleService-Info.plist", "GoogleService-Info.plist"));
			// Write PBXProject object back to the file
			proj.WriteToFile(projPath);
		}
	}
}
 
#endif