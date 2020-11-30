using API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;

namespace API.Services
{

    /// <summary>
    /// Classe responsável em gerenciar o TimerActivity
    /// 
    /// Descrição da Funcionalidade:
    /// Usuário irá iniciar um timer para executar uma determinada atividade.
    /// O problema é, se ele fechar o browser sem que seja feito a requisição para finalizar o timer
    /// </summary>
    public class ActivityStartedService
    {
        private static Timer Timer;
        private static readonly DirectoryInfo DirectoryInfo = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "Activies"));
        public static bool TimerActive => Timer != null && Timer.Enabled;
        private static List<PingTimeActivity> PingTimes;


        /// <summary>
        /// Metodo responsável em fazer a verificação e os backups das atividades ativas
        /// 
        /// ideia é ir atualizando os pings das atividades na propriedade static "PingTimes" a cada requisição do usuario
        /// isso em intervalos curtos
        /// 
        /// dentro do evento do timer terá uma verificação se o ultimo ping do usuario está com intervalo maior que o esperado,
        /// caso estiver, será definido que o usuario não esta mais executando a atividade.
        /// 
        /// </summary>
        public static void StartTimer()
        {
            Directory.CreateDirectory(DirectoryInfo.FullName);

            ReadAllFiles();

            Timer = new Timer
            {
                Interval = 50000,
                AutoReset = true,
                Enabled = true
            };

            Timer.Elapsed += TimerEvent;
        }

        public static void StopTimer()
        {
            if (TimerActive)
            {
                Timer.Stop();
            }
        }

        public void Add(PingTimeActivity pingTimeActivity)
        {
            PingTimes.Add(new PingTimeActivity
            {
                UserId = pingTimeActivity.UserId,
                LastPing = pingTimeActivity.LastPing
            });

            WriteAllFile(pingTimeActivity.UserId, pingTimeActivity.LastPing);
        }

        private static void TimerEvent(object source, ElapsedEventArgs e)
        {
            ReadAllFiles();
        }

        private static void ReadAllFiles()
        {
            PingTimes = new List<PingTimeActivity>();

            FileInfo[] fileInfos = DirectoryInfo.GetFiles();

            foreach (var item in fileInfos)
            {
                using StreamReader streamReader = new StreamReader(item.FullName);
                string result = streamReader.ReadToEnd();
                streamReader.Close();

                PingTimes.Add(new PingTimeActivity
                {
                    UserId = int.Parse(item.Name),
                    LastPing = DateTime.Parse(result)
                });
            }
        }

        private static void WriteAllFile(int userId, DateTime lastPing)
        {
            using StreamWriter streamWriter = new StreamWriter(Path.Combine(DirectoryInfo.FullName, $"{userId}.txt"));
            streamWriter.Write(lastPing);
            streamWriter.Close();
        }
    }
}
