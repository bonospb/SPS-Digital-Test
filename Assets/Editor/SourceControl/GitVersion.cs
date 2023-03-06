namespace FreeTeam.BP.SourceControl
{
	public class GitVersion : SourceControlVersion
	{
#if UNITY_EDITOR_WIN
		protected override string Executable => "git.exe";
#else
		protected override string Executable => "git";
#endif
		protected override string Command => "rev-list --count HEAD";
	}
}
