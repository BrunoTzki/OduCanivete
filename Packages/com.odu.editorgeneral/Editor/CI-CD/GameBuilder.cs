using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;

namespace OduLib.Canivete.EditorGeneral
{
    public class GameBuilder
    {
        /// <summary>
        /// <para><b>Nome base do build</b></para>
        /// <para>
        /// Utilizado como nome do executável gerado durante o processo de build.
        /// </para>
        /// </summary>
        public static string BuildName = "Odu";

        /// <summary>
        /// <para><b>Build manual para Windows (64 bits)</b></para>
        /// <para>
        /// Executa o processo de build do jogo para a plataforma
        /// <see cref="BuildTarget.StandaloneWindows64"/>.
        /// </para>
        ///
        /// <para><b>Acesso</b></para>
        /// <para>
        /// Disponível no menu do Unity Editor em:
        /// <c>Odu / Build / Windows</c>
        /// </para>
        ///
        /// <para><b>Fluxo do build</b></para>
        /// <list type="number">
        /// <item>
        /// <description>Coleta as cenas configuradas no <see cref="EditorBuildSettings"/>.</description>
        /// </item>
        /// <item>
        /// <description>Define as opções de build e o caminho de saída.</description>
        /// </item>
        /// <item>
        /// <description>Executa o build via <see cref="BuildPipeline.BuildPlayer"/>.</description>
        /// </item>
        /// <item>
        /// <description>Registra o resultado no console.</description>
        /// </item>
        /// </list>
        ///
        /// <para><b>Observações importantes</b></para>
        /// <para>
        /// Apenas cenas marcadas como <i>enabled</i> no Build Settings
        /// devem ser incluídas no build final.
        /// </para>
        /// </summary>
        [MenuItem("Odu/Build/Windows")]
        public static void PerformBuildWindows()
        {
            var buildOptions = new BuildPlayerOptions();
            string[] scenePaths = new string[EditorBuildSettings.scenes.Length];

            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                var scene = EditorBuildSettings.scenes[i];

                if (!scene.enabled)
                {
                    continue;
                }

                scenePaths[i] = scene.path;
            }

            buildOptions.scenes = scenePaths;
            buildOptions.locationPathName = $"build/windows/{BuildName}.exe";
            buildOptions.target = BuildTarget.StandaloneWindows64;
            buildOptions.options = BuildOptions.None;

            Debug.Log(buildOptions.locationPathName);

            var buildReport = BuildPipeline.BuildPlayer(buildOptions);
            var summary = buildReport.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"Build succeeded: {summary.totalSize} bytes");
            }
            else if (summary.result == BuildResult.Failed)
            {
                Debug.Log("Build failed");
            }
        }
    }
}