namespace OduLib.Canivete.Audio
{
    /// <summary>
    /// <para><b>Responsabilidade</b></para>
    /// <para>
    /// Fornece utilidades globais para controle de áudio via FMOD,
    /// permitindo interromper todos os sons atualmente em reprodução.
    /// </para>
    ///
    /// <para><b>Quando usar</b></para>
    /// <list type="bullet">
    /// <item>
    /// <description>Transições abruptas de cena.</description>
    /// </item>
    /// <item>
    /// <description>Reset completo do estado de áudio do jogo.</description>
    /// </item>
    /// <item>
    /// <description>Entrada em menus, pausa global ou encerramento de gameplay.</description>
    /// </item>
    /// </list>
    ///
    /// <para><b>Dependências</b></para>
    /// <para>
    /// Este sistema depende do <b>FMOD Unity Integration</b>
    /// e assume que os bancos já estão carregados no momento da chamada.
    /// </para>
    ///
    /// <para><b>Observações importantes</b></para>
    /// <para>
    /// Esta operação é global e afeta todos os <c>Buses</c> de todos os bancos carregados.
    /// Use com cautela, pois não distingue categorias ou contextos específicos.
    /// </para>
    /// </summary>
    public class AudioEventsControl
    {
        /// <summary>
        /// <para><b>Interrupção global de áudio</b></para>
        /// <para>
        /// Percorre todos os bancos de áudio carregados no FMOD
        /// e interrompe todos os eventos ativos em seus respectivos buses.
        /// </para>
        ///
        /// <para><b>Comportamento de parada</b></para>
        /// <para>
        /// Utiliza <see cref="FMOD.Studio.STOP_MODE.ALLOWFADEOUT"/>,
        /// permitindo que os sons finalizem suavemente.
        /// </para>
        /// </summary>
        public static void StopAllSounds()
        {
            FMODUnity.RuntimeManager.StudioSystem.getBankList(out var bankList);

            foreach (var bank in bankList)
            {
                bank.getBusList(out var buses);

                if (buses.Length > 0)
                {
                    foreach (var bus in buses)
                    {
                        bus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    }
                }
            }
        }
    }
}
