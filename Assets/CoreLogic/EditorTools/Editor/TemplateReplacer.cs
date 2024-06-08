//==========================
// - FileName: TemplateReplacer.cs
// - Created: AjieloA
// - CreateTime: 2024-06-08 18:40:52
// - Email: 1758580256@qq.com
// - Description:�ű�����ģ���滻
//==========================
using System.IO;
using System;
using UnityEditor;

[Obsolete]
public class TemplateReplacer : AssetModificationProcessor
{
    // �����ű�ʱ����
    public static void OnWillCreateAsset(string path)
    {
        // ֻ���� C# �ű��ļ�
        path = path.Replace(".meta", "");
        if (path.EndsWith(".cs"))
        {
            // �ӳٵ�����ȷ���ļ��������
            EditorApplication.delayCall += () => ProcessScript(path);
        }
    }

    private static void ProcessScript(string path)
    {
        // ��ȡ�ű�����
        string content = File.ReadAllText(path);

        // ��ȡ��ǰ����
        string currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // �滻ռλ��
        content = content.Replace("#AUTHORNAME#", "AjieloA"); // �滻Ϊʵ����������
        content = content.Replace("#CREATIONDATE#", currentDate);
        content = content.Replace("#AUTHOREMAIL#", "1758580256@qq.com");

        // д���ļ�
        File.WriteAllText(path, content);

        // ˢ���ʲ����ݿ�
        AssetDatabase.Refresh();
    }
}
