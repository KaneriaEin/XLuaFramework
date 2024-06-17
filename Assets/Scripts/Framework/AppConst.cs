
public enum GameMode
{
    EditorMode,
    PackageBundle,
    UpdateMode,
}

public class AppConst
{
    public const string BundleExtension = ".ab";
    public const string FileListName = "filelist.txt";

    public static GameMode GameMode = GameMode.EditorMode;
    public static bool OpenLog = true;
    //�ȸ���Դ�ĵ�ַ
    public const string ResourcesUrl = "http://127.0.0.1/AssetBundles";
}