
using MindTouch.Tasking;
using FoireMuses.Core;
using System;
using LoveSeat;
using Autofac.Builder;
using MindTouch.Xml;
using Newtonsoft.Json.Linq;

namespace FoireMuses.ConsoleTest
{

	using Yield = System.Collections.Generic.IEnumerator<MindTouch.Tasking.IYield>;

	class Program
	{
		private static readonly log4net.ILog theLogger = log4net.LogManager.GetLogger(typeof(Program));

		static void Main(string[] args)
		{
			int? a = 2;
			Coroutine.Invoke(StartTests, a, new Result<int?>()).WhenDone(
				b =>
				{
					Console.WriteLine("Success main");
					Console.WriteLine(b.ToString());
					Console.ReadLine();
				},
				e => {
					Console.WriteLine("Exception main");
					throw e; 
				}
				);
			Console.ReadLine();

		}

		static  Yield StartTests(int? a, Result<int?> aResult)
		{
			Result<int?> test = new Result<int?>();
			Console.WriteLine("Avant le yield boolean");
			Result<bool> testbool = new Result<bool>();
			yield return Booll(testbool);
			if (testbool.Value)
			{
				Console.WriteLine("false je throw argumentException");
				aResult.Throw(new ArgumentException());
				yield break;
			}
			Result<bool> blabla = new Result<bool>();
			Console.WriteLine("avant blabla");
			yield return Coroutine.Invoke(StartTests2,blabla);
			Console.WriteLine("Apres le yield - start tests2");
			Console.WriteLine("valeur de blabla.Value quoi qu'il arrive:" + blabla.Value);
			Console.WriteLine("Valeur de blabla.HasValue:" + blabla.HasValue);
			yield return Calcul(a, test);
			Console.WriteLine("Apres le yield - start tests");
			Console.WriteLine("valeur de a.Value quoi qu'il arrive:" + a.Value);
			Console.WriteLine("Valeur de a.HasValue:" + a.HasValue);
			aResult.Return(a.Value);
			Console.WriteLine("Après le aResult.Return(..)");
		}

		static Yield StartTests2(Result<bool> aResult)
		{
			Result<bool> test = new Result<bool>();
			Console.WriteLine("Avant le yield boolean");
			Result<bool> testbool = new Result<bool>();
			yield return Booll(test);
			yield return Booll(testbool);
			if (test.Value && testbool.Value)
				aResult.Return(true);
			else
				aResult.Return(false);
		}

		static Result<bool> Booll(Result<bool> aResult)
		{
			aResult.Return(false);
			return aResult;
		}

		static Result<int?> Calcul(int? a, Result<int?> aResult)
		{
			if (a == null)
			{
				Console.WriteLine("a est null je throw une exception");
				aResult.Throw(new ArgumentNullException());
			}
			else
			{
				aResult.Return(a * a);
				Console.WriteLine("Je renvoie a*a");
			}
			return aResult;
		}




	}
}
