using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace Importer
{
    public class Importer
    {
        SQLiteConnection _connection;

        public Importer()
        {
            _connection = new SQLiteConnection("Data Source=..\\..\\..\\Droid\\Data\\OneSet.db3;Version=3");
            _connection.Open();
            //_connection = new SQLiteConnection("C:\\dev\\1Set\\Droid\\Data\\OneSet.db3");
        }

        public void Start(TextBox textBox)
        {
            var counter = 0;
            var line = string.Empty;

            var file = new StreamReader("..\\..\\..\\Droid\\Data\\excel.txt");
            while ((line = file.ReadLine()) != null)
            {
                counter++;
                line = line.Trim();
                if (line == string.Empty)
                {
                    continue;
                }

                var tokens = line.Split(new char[]{'\t'}, StringSplitOptions.None);
                if (tokens.Length == 0)
                {
                    continue;
                }

                int num;
                if (!int.TryParse(tokens[0], out num))
                {
                    continue;
                }

                DateTime created;
                if (!DateTime.TryParse(tokens[tokens.Length - 1], out created))
                {
                    continue;
                }

                var index = 0;
                InsertWorkout(created, 1, GetReps(tokens[index]), GetWeight(tokens[index + 1]));
                
                index = index + 2;
                InsertWorkout(created, 2, GetReps(tokens[index]), GetWeight(tokens[index + 1]));

                index = index + 2;
                InsertWorkout(created, 3, GetReps(tokens[index]), GetWeight(tokens[index + 1]));

                index = index + 2;
                InsertWorkout(created, 4, GetReps(tokens[index]), GetWeight(tokens[index + 1]));

                index = index + 2;
                InsertWorkout(created, 5, GetReps(tokens[index]), GetWeight(tokens[index + 1]));

                index = index + 2;
                InsertWorkout(created, 6, GetReps(tokens[index]), GetWeight(tokens[index + 1]));

                textBox.AppendText(string.Format("Line: {0}: {1} \n", counter, line));
            }

            file.Close();
        }

        private int GetReps(string token)
        {
            int num;
            if (!int.TryParse(token, out num))
            {
                throw new Exception("Cannot convert number '" + token + "'");
            }
            return num;
        }
        private double GetWeight(string token)
        {
            double num;
            if (!double.TryParse(token, out num))
            {
                throw new Exception("Cannot convert number '" + token + "'");
            }
            return num;
        }

        private void InsertWorkout(DateTime created, int exerciseId, int reps, double weight)
        {
            var sql = string.Format("insert into workouts (Created, ExerciseId, Reps, Weight) values ({0}, {1}, {2}, {3})", created.Ticks, exerciseId, reps, weight);
            var command = new SQLiteCommand(sql, _connection);
            command.ExecuteNonQuery();
        }
    }
}
