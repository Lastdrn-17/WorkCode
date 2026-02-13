using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GeniIdiot
{
    public class Question
    {
        public string Text;
        public string Answer;
    }
    public class User 
    {
        public string Name;
        public string Diagnos;
        public int NumTrueAns;
    }
    internal class Program
    {
        private static List<User> allUsers = new List<User>();
        private static List<Question> questions = new List<Question>();
        static void Main()
        {
            bool isWorking = true;

            while (isWorking)
            {
                Console.Write("Привет, введите свое имя: ");
                string userName = Console.ReadLine();
                Console.WriteLine();                

                if (userName == "2106")
                {
                    AdminMode();
                    continue;
                }
                List<User> userList = new List<User>();
                User currentUser = new User();
                currentUser.Name = userName;
                userList.Add(currentUser);

                Console.WriteLine($"Добро пожаловать в викторину, {userName}!");
                Console.WriteLine("Выберите дальнейшие действия");
                Console.WriteLine("1) Начать игру");
                Console.WriteLine("2) Посмотреть результаты");
                Console.WriteLine("3) Выход");
                int choice = int.Parse(Console.ReadLine());
                int correctAnswers = 0;
                string diagnos = "";
                switch (choice)
                {
                    case 1:
                        Console.WriteLine();
                        Console.WriteLine("Отвечайте на вопросы. После каждого ответа нажимайте Enter.");
                        Console.WriteLine();
                        correctAnswers = AnswerQuestions();
                        diagnos = EndGame(correctAnswers, userName);
                        currentUser.NumTrueAns = correctAnswers;
                        currentUser.Diagnos = diagnos;
                        allUsers.Add(currentUser);
                        SaveGameResults(userName, correctAnswers, diagnos);

                        Console.WriteLine("Что дальше?");
                        Console.WriteLine("1) Пройти тест еще раз");
                        Console.WriteLine("2) Вернуться в главное меню");
                        Console.WriteLine("3) Посмотреть результаты");
                        Console.WriteLine("4) Выход");
                        int repeatChoice = int.Parse(Console.ReadLine());
                        Console.WriteLine();
                        switch (repeatChoice)
                        {
                            case 1:
                                continue;
                            case 2:
                                break;
                            case 3:
                                ShowAllResults();
                                break;
                            case 4:
                                isWorking = false;
                                break;
                            default:
                                Console.WriteLine("Неверный выбор.");
                                break;
                        }
                        break;

                    case 2:
                        ShowAllResults();
                        break;

                    case 3:
                        isWorking = false;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }

            Console.WriteLine("До свидания!");
        }       

        //////
        static int AnswerQuestions()
        {
            List<Question> questions = new List<Question>();

            Question question1 = new Question();
            question1.Text = "Сколько будет 2 + 2?";
            question1.Answer = "4";
            questions.Add(question1);

            Question question2 = new Question();
            question2.Text = "Какой город является столицей Франции?";
            question2.Answer = "Париж";
            questions.Add(question2);

            Question question3 = new Question();
            question3.Text = "Как обозначается химический элемент кислород?";
            question3.Answer = "0";
            questions.Add(question3);

            Question question4 = new Question();
            question4.Text = "Сколько планет в Солнечной системе?";
            question4.Answer = "8";
            questions.Add(question4);

            int countCorrectAnswers = 0;

            for (int i = 0; i < questions.Count; i++)
            {
                int randomIndex = new Random().Next(0, questions.Count);
                Console.Write($"{i}");
                Console.WriteLine(questions[randomIndex].Text);
                string userAnswer = Console.ReadLine();
                if (userAnswer.Trim().ToLower() == questions[randomIndex].Answer)
                {
                    countCorrectAnswers++;
                }
                questions.RemoveAt(randomIndex);
            }

            return countCorrectAnswers;
        }   

        //////
        static string EndGame(int correctAnswers, string userName)
        {
            Console.WriteLine("Викторина завершена!");
            Console.WriteLine($"Вы ответили правильно на {correctAnswers} из {questions.Count} вопросов.");
            double percent = Math.Round((double)correctAnswers / questions.Count * 100, 1);
            string diagnos;
            if (percent <= 20)
            {
                diagnos = "идиот";
            }
            else if (percent <= 40)
            {
                diagnos = "кретин";
            }
            else if (percent <= 60)
            {
                diagnos = "обучаемый дурак";
            }
            else if (percent <= 80)
            {
                diagnos = "талант";
            }
            else
            {
                diagnos = "гений";
            }
            Console.WriteLine($"{userName}, ваш результат: {diagnos}");
            return diagnos;
        }

        //////
        static void SaveGameResults(string userName, int correctAnswer, string diagnos)
        {
            User newUser = new User
            {
                Name = userName,
                NumTrueAns = correctAnswer,
                Diagnos = diagnos
            };
            allUsers.Add(newUser);
            string filePath = "..\\..\\..\\result.txt";
            string allResults = $"{userName}#{correctAnswer}#{diagnos}";
            File.AppendAllText(filePath, allResults + Environment.NewLine);
        }

        //////
        static void ShowAllResults()
        {
            string filePath = "..\\..\\..\\result.txt";            
            string[] fileData = File.ReadAllLines(filePath);

            if (fileData.Length == 0)
            {
                Console.WriteLine("Нет сохранённых результатов.");
                return;
            }
            Console.WriteLine("--- ВСЕ РЕЗУЛЬТАТЫ ---");

            for (int i = 0; i < fileData.Length; i++)
            {
                string line = fileData[i];
                string[] dannye = line.Split('#');

                if (dannye.Length == 3)
                {
                    Console.WriteLine($"№{i + 1}: имя: {dannye[0]} — {dannye[1]} правильных ответов, диагноз: {dannye[2]}");
                }
            }

            Console.WriteLine();
        }

        //////
        static void AdminMode()
        {
            bool adminMenuActive = true; 
            while (adminMenuActive)
            {
                Console.WriteLine("---РЕЖИМ АДМИНИСТРАТОРА---");
                Console.WriteLine("1) Добавить новый вопрос");
                Console.WriteLine("2) Удалить вопрос");
                Console.WriteLine("3) Выйти в главное меню");
                int choice = int.Parse(Console.ReadLine());
                Console.WriteLine();

                switch (choice)
                {
                    case 1:
                        AddQuestion();
                        break;

                    case 2:
                        RemoveQuestion();
                        break;

                    case 3:
                        Console.WriteLine("Выход в главное меню...");
                        Console.WriteLine();
                        adminMenuActive = false;
                        break;
                }
            }
        }

        //////
        static void AddQuestion()
        {
            Console.Write("Введите вопрос: ");
            Console.WriteLine("(Если хотите вернуться введите цифру 0)");
            string newQuestion = Console.ReadLine();
            if (newQuestion == "0")
            {
                Console.WriteLine("Выход без добавления");
                Console.WriteLine();
                return;
            }
            List<Question> questions = new List<Question>();
            Question newQuestion1 = new Question();
            newQuestion1.Text = newQuestion;            
            Console.WriteLine();            
            Console.Write("Введите правильный ответ: ");
            string newAnswer = Console.ReadLine();
            newQuestion1.Answer = newAnswer;
            questions.Add(newQuestion1);
            Console.WriteLine("Вопрос успешно добавлен!");
            Console.WriteLine();
        }

        //////
        static void RemoveQuestion()
        {
            if (questions.Count == 0)
            {
                Console.WriteLine("Список вопросов пуст. Удалять нечего.");
                return;
            }

            Console.WriteLine("Текущий список:");
            for (int i = 0; i < questions.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {questions[i]}");
            }

            Console.WriteLine();
            Console.WriteLine("Выберите номер удаляемого вопроса:");
            Console.WriteLine("(Если хотите вернуться введите цифру 0)");
            int numDelete;
            while (true)
            {
                string input = Console.ReadLine();
                if (!int.TryParse(input, out numDelete))
                {
                    Console.WriteLine("Ошибка: введите числовое значение.");
                    continue;
                }

                if (numDelete == 0)
                {
                    Console.WriteLine("Выход без удаления");
                    Console.WriteLine();
                    return;
                }

                if (numDelete < 1 || numDelete > questions.Count)
                {
                    Console.WriteLine($"Ошибка: номер должен быть от 1 до {questions.Count}. Попробуйте ещё раз.");
                    continue;
                }                
                break;
            }
            questions.RemoveAt(numDelete - 1);
            Console.WriteLine("Успешно удалено!");
        }

    }
}
