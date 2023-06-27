namespace CoFramework.ResourceManagement
{

    public enum EResourceMode
    {
        EditorSimulateMode,
        OfflinePlayMode
    }
    public class ResModuleCreateParameters : CreateParameters
    {
        public string DefaultPackageName = "DefaultPackage";

        public EResourceMode Mode = EResourceMode.EditorSimulateMode;
    }
}