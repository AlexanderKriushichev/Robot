using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LabirintGenerator
{
    //Обозначения:
    //0 - посещенная свободная для прохода клетка
    //1 - стена
    //2 - старт
    //3 - финиш
    //5 - непосещенная свободная клетка

	class Program
	{
		private static List<List<int>> labirint;
		private static Stack<Point> path = new Stack<Point>();
		private static List<Directions> direction;

		private static int height;
		private static int width;

		static int tmpN;
		static int tmpM;

		private static Point currentPoint;
		private static Point startPoint;
		private static Point finishPoint;

		private static string filePath;

		static void Main(string[] args)
		{

			int passWidth = 2;
			int wallWidth = 1;
			int labirintWidth = 10;
            int labirintHeigth = 10;

            tmpN = labirintHeigth / (passWidth + wallWidth);
			tmpM = labirintWidth / (passWidth + wallWidth);

			height = 2 * tmpN + 1;
			width = 2 * tmpM + 1;

			int a = tmpN - 1;
			int b = 0;
			int c = tmpN - 1;
			int d = tmpM - 1;

			startPoint.X = a * 2 + 1;
			startPoint.Y = b * 2 + 1;
			finishPoint.X = c * 2 + 1;
			finishPoint.Y = d * 2 + 1;

			currentPoint = startPoint;
			direction = new List<Directions>();
			GenerateLabirint();
			Console.WriteLine("Укажите имя лабиринта: ");
			filePath = Console.ReadLine();
			SaveToRoboLabs(passWidth, wallWidth);
			SaveAsPicture(labirint);
		}

        /// <summary>
        /// Генерация лабиринта
        /// </summary>
		static void GenerateLabirint()
		{
			GenerateStartLabirint();
			path.Push(currentPoint);
			bool isStart = true;
			Random r = new Random();
			while (path.Count > 0)
			{
				if (isStart)
				{
					path.Pop();
					isStart = false;
				}
				GetDirection();
				if (direction.Count > 0)
				{
					Directions next = direction[r.Next(0, direction.Count)];
					switch (next)
					{
						case Directions.Left:
							if (!MoveLeft())
								currentPoint = path.Pop();
							break;
						case Directions.Right:
							if (!MoveRight())
								currentPoint = path.Pop();
							break;
						case Directions.Up:
							if (!MoveUp())
								currentPoint = path.Pop();
							break;
						case Directions.Down:
							if (!MoveDown())
								currentPoint = path.Pop();
							break;
					}
					direction.Clear();
				}
				else
					currentPoint = path.Pop();
			}
		}

		static void MazeFromFile(string f)
		{
			labirint = File.ReadAllLines(f).Select(l => l.Select(i => Int32.Parse(i.ToString())).ToList()).ToList();
		}		

        /// <summary>
        /// Генерирует стартовый лабиринт
        /// </summary>
		static void GenerateStartLabirint()
		{
			labirint = new List<List<int>>(height);
			for (int i = 0; i < height; ++i)
			{
				labirint.Add(new List<int>(width));
				for (int j = 0; j < width; ++j)
					labirint[i].Add(i % 2 == 0 || j % 2 == 0 ? 1 : 5);
			}
			labirint[startPoint.X][startPoint.Y] = 2;
			labirint[finishPoint.X][finishPoint.Y] = 5;
		}

		static void PrintLairint()
		{
			for (int i = 0; i < height; ++i)
			{
				for (int j = 0; j < width; ++j)
					Console.Write(labirint[i][j]);
				Console.Write("\n");
			}
		}

		static void SaveAsPicture(List<List<int>> mz)
		{
			Bitmap pic = new Bitmap(mz[0].Count, mz.Count);
			using (var g = Graphics.FromImage(pic))
				g.Clear(Color.White);

			for (int i = 0; i < mz.Count; ++i)
			{
				for (int j = 0; j < mz[0].Count; ++j)
				{
					switch (mz[i][j])
					{
						case 1:
							pic.SetPixel(j, i, Color.Black);
							break;
						case 2:
							pic.SetPixel(j, i, Color.GreenYellow);
							break;
						case 3:
							pic.SetPixel(j, i, Color.Red);
							break;
					}
				}
			}
			pic.Save(filePath + ".png", ImageFormat.Png);
		}

		static void SaveAsText(List<List<int>> mz)
		{
			using (StreamWriter f =
			new StreamWriter(filePath + ".txt"))
			{
				for (int i = 0; i < mz.Count; ++i)
				{
					for (int j = 0; j < mz[0].Count; ++j)
						f.Write(mz[j][i]);
                    f.WriteLine("");
				}
			}
		}

		/// <summary>
		/// Класс для сохранения лабиринта по спецификации лаб
		/// </summary>
		/// <param name="freeWidth">ширина прохода</param>
		/// <param name="filledWidth">ширина стен</param>
		static void SaveToRoboLabs(int freeWidth, int filledWidth)
		{
			List<List<int>> res = new List<List<int>>();

			for (int i = 0; i < height; ++i)
			{
				res.Add(new List<int>());
				for (int j = 0; j < width; ++j)
				{
					for (int k = 0; k < (j % 2 == 1 ? freeWidth : filledWidth); ++k)
						res[res.Count - 1].Add(labirint[i][j]);
				}
				for (int k = 0; k < (i % 2 == 1 ? freeWidth - 1 : filledWidth - 1); ++k)
				{
					res.Add(res[res.Count - 1]);
				}
			}
			SaveAsPicture(res);
			SaveAsText(res);
		}

        /// <summary>
        /// Передвинуться влево
        /// </summary>
        /// <returns></returns>
		static bool MoveLeft()
		{
			var tmp = new Point(currentPoint.X - 2, currentPoint.Y);
			labirint[currentPoint.X - 1][currentPoint.Y] = 0;
			MadeVisited(tmp);
			if (tmp == finishPoint)
			{
				labirint[tmp.X - 2][tmp.Y] = 3;
				return false;
			}
			currentPoint = tmp;
			return true;
		}

        /// <summary>
        /// Передвинуться вправо
        /// </summary>
        /// <returns></returns>
		static bool MoveRight()
		{
			var tmp = new Point(currentPoint.X + 2, currentPoint.Y);
			labirint[currentPoint.X + 1][currentPoint.Y] = 0;
			MadeVisited(tmp);
			if (tmp == finishPoint)
			{
				labirint[currentPoint.X + 2][currentPoint.Y] = 3;
				return false;
			}
			currentPoint = tmp;
			return true;
		}

        /// <summary>
        /// Передвинуться вверх
        /// </summary>
        /// <returns></returns>
		static bool MoveUp()
		{
			var tmp = new Point(currentPoint.X, currentPoint.Y - 2);
			labirint[currentPoint.X][currentPoint.Y - 1] = 0;
			MadeVisited(tmp);
			if (tmp == finishPoint)
			{
				labirint[currentPoint.X][currentPoint.Y - 2] = 3;
				return false;
			}
			if (tmp == finishPoint)
				return false;
			currentPoint = tmp;
			return true;
		}

        /// <summary>
        /// Передвинуться вниз
        /// </summary>
        /// <returns></returns>
		static bool MoveDown()
		{
			var tmp = new Point(currentPoint.X, currentPoint.Y + 2);
			labirint[currentPoint.X][currentPoint.Y + 1] = 0;
			MadeVisited(tmp);
			if (tmp == finishPoint)
			{
				labirint[currentPoint.X][currentPoint.Y + 2] = 3;
				return false;
			}
			currentPoint = tmp;
			return true;
		}

        /// <summary>
        /// Получить список возможных направлений
        /// </summary>
		static void GetDirection()
		{
			if (currentPoint.X - 2 > 0 && !CheckVisit(new Point(currentPoint.X - 2, currentPoint.Y)))
				direction.Add(Directions.Left);
			if (currentPoint.X + 2 < height && !CheckVisit(new Point(currentPoint.X + 2, currentPoint.Y)))
				direction.Add(Directions.Right);
			if (currentPoint.Y - 2 > 0 && !CheckVisit(new Point(currentPoint.X, currentPoint.Y - 2)))
				direction.Add(Directions.Up);
			if (currentPoint.Y + 2 < width && !CheckVisit(new Point(currentPoint.X, currentPoint.Y + 2)))
				direction.Add(Directions.Down);
			if (direction.Count > 1)
				path.Push(currentPoint);
		}

		enum Directions
		{
			Left,
			Right,
			Up,
			Down
		}


        /// <summary>
        /// Пометить точку как посещенную
        /// </summary>
        /// <param name="p"></param>
		static void MadeVisited(Point p)
		{
			labirint[p.X][p.Y] = 0;
		}

        /// <summary>
        /// Проверить точку на посещение
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
		static bool CheckVisit(Point p)
		{
			int t = labirint[p.X][p.Y];
			return t == 0 || t == 2 || t == 3;
		}
	}
}
