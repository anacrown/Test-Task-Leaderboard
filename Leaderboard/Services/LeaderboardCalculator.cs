﻿using Leaderboard.Models;

// ReSharper disable once InvalidXmlDocComment
/**
 * ТРЕБОВАНИЕ:
 *
 * Необходимо реализовать функцию CalculatePlaces.
 * Функция распределяет места пользователей, учитывая ограничения для получения первых мест и набранные пользователями очки.
 * Подробное ТЗ смотреть в readme.txt
 */

/**
 * ТЕХНИЧЕСКИЕ ОГРАНИЧЕНИЯ:
 *
 * количество очков это всегда целое положительное число
 * FirstPlaceMinScore > SecondPlaceMinScore > ThirdPlaceMinScore > 0
 * в конкурсе участвует от 1 до 100 пользователей
 * 2 пользователя не могут набрать одинаковое количество баллов (разные баллы у пользователей гарантируются бизнес-логикой, не стоит усложнять алгоритм)
 * нет ограничений на скорость работы функции и потребляемую ей память
 * при реализации функции разрешается использование любых библиотек, любого стиля написания кода
 * в функцию передаются только валидные данные, которые соответствуют предыдущим ограничениям (проверять это в функции не нужно)
 */

/**
 * ВХОДНЫЕ ДАННЫЕ:
 *
 * usersWithScores - это список пользователей и заработанные каждым из них очки,
 * это неотсортированный массив вида [{userId: "id1", score: score1}, ... , {userId: "idn", score: scoreN}], где score1 ... scoreN различные положительные целые числа, id1 ... idN произвольные неповторяющиеся идентификаторы
 *
 * leaderboardMinScores - это значения минимального количества очков для первых 3 мест
 * это объект вида { FirstPlaceMinScore: score1, SecondPlaceMinScore: score2, ThirdPlaceMinScore : score3 }, где score1 > score2 > score3 > 0 целые положительные числа
 */

/**
 * РЕЗУЛЬТАТ:
 *
 * Функция должна вернуть пользователей с занятыми ими местами
 * Массив вида (сортировка массива не важна): [{UserId: "id1", Place: user1Place}, ..., {UserId: "idN", Place: userNPlace}], где user1Place ... userNPlace это целые положительные числа равные занятым пользователями местами, id1 ... idN идентификаторы пользователей из массива users
 */


namespace Leaderboard.Services
{
    public class LeaderboardCalculator : ILeaderboardCalculator
    {
        public IEnumerable<UserWithPlace> CalculatePlaces(IEnumerable<IUserWithScore> usersWithScores, LeaderboardMinScores leaderboardMinScores)
        {
            var leaderBoard = new Dictionary<int, IUserWithScore>();

            foreach (var user in usersWithScores)
            {
                if (user.Score >= leaderboardMinScores.FirstPlaceMinScore)
                {
                    TakePlace(1, user, leaderBoard);
                }
                else if (user.Score >= leaderboardMinScores.SecondPlaceMinScore)
                {
                    TakePlace(2, user, leaderBoard);
                }
                else if (user.Score >= leaderboardMinScores.ThirdPlaceMinScore)
                {
                    TakePlace(3, user, leaderBoard);
                }
                else
                {
                    TakePlace(4, user, leaderBoard);
                }
            }

            return leaderBoard
                .Select(p => new UserWithPlace(p.Value.UserId, p.Key));
        }

        private static void TakePlace(int place, IUserWithScore user, IDictionary<int, IUserWithScore> leaderBoard)
        {
            while (true)
            {
                if (leaderBoard.TryGetValue(place, out var u))
                {
                    if (u.Score < user.Score)
                    {
                        TakePlace(place + 1, u, leaderBoard);
                        leaderBoard[place] = user;
                    }
                    else
                    {
                        place += 1;
                        continue;
                    }
                }
                else
                    leaderBoard.Add(place, user);

                break;
            }
        }
    }
}