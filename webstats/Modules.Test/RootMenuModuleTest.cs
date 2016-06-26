/*
 * Created by SharpDevelop.
 * User: Lars Magnus
 * Date: 12.06.2014
 * Time: 22:50
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;
using Shouldly;

namespace Modules.Test
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	[TestFixture]
	[Ignore("Cannot get it to work")]
	public class RootMenuModelsTest
	{
		[Test]
		public void Test_ShouldReturnStatusOk_WhenRouteExists()
		{
			var bootstrapper = new Bootstrapper();
			var browser = new Browser(bootstrapper);
			
			var result = browser.Get("/", with => {
				with.HttpRequest();
			});
		
			result.StatusCode.ShouldBe(HttpStatusCode.OK);
		}
	}
}