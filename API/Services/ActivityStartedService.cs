using API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private static DateTime DateTimeBackup;
        private static readonly DirectoryInfo DirectoryInfo = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "Activies"));
        public static bool TimerActive => Timer != null && Timer.Enabled;
        public static List<PingTimeActivity> PingTimes { get; private set; }


        /// <summary>
        /// Metodo responsável em fazer a verificação e os backups das atividades ativas
        /// 
        /// ideia é ir atualizando os pings das atividades na propriedade static "PingTimes" a cada requisição do usuario
        /// isso em intervalos curtos
        /// </summary>
        public static void StartTimer()
        {
            Directory.CreateDirectory(DirectoryInfo.FullName);

            ReadAllFiles();
            DateTimeBackup = DateTime.Now.AddHours(1);

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

        /// <summary>
        /// Encerra a atividade do usuario; metodo deve ser chamado junto com o update na base de dados
        /// </summary>
        /// <param name="userId"></param>
        public void FinishActivity(int userId)
        {
            var ping = PingTimes.FirstOrDefault(x => x.UserId == userId);

            PingTimes.Remove(ping);
        }

        /// <summary>
        /// Inicia uma nova atividade do usuario
        /// </summary>
        /// <param name="pingTimeActivity"></param>
        public void Add(PingTimeActivity pingTimeActivity)
        {
            PingTimes.Add(new PingTimeActivity
            {
                UserId = pingTimeActivity.UserId,
                Pings = pingTimeActivity.Pings,
                Finished = false
            });
        }

        /// <summary>
        /// Evento do timer; verifica momento de fazer o backup
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private static void TimerEvent(object source, ElapsedEventArgs e)
        {
            if (DateTime.Now >= DateTimeBackup)
            {
                DateTimeBackup = DateTimeBackup.AddMinutes(10);
                Backup();
            }
        }

        /// <summary>
        /// Realiza o backup
        /// </summary>
        private static void Backup()
        {
            foreach (var item in PingTimes)
            {
                string values = string.Empty;
                item.Pings.ForEach(x => values += $"{x}++!");

                using StreamWriter streamWriter = new StreamWriter(Path.Combine(DirectoryInfo.FullName, $"{item.UserId}.txt"));
                streamWriter.Write(values);
                streamWriter.Close();
            }
        }

        /// <summary>
        /// Inicia uma nova list pingTimeActivity com os registros do backup
        /// </summary>
        private static void ReadAllFiles()
        {
            PingTimes = new List<PingTimeActivity>();

            FileInfo[] fileInfos = DirectoryInfo.GetFiles();

            foreach (var item in fileInfos)
            {
                using StreamReader streamReader = new StreamReader(item.FullName);
                string result = streamReader.ReadToEnd();
                streamReader.Close();

                string[] values = result.Split("++!");

                var ping = new PingTimeActivity
                {
                    UserId = int.Parse(item.Name),
                    Pings = new List<DateTime>()
                };

                foreach (var value in values)
                {
                    ping.Pings.Add(DateTime.Parse(value));
                }
            }
        }
    }
}
