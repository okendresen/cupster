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

namespace Modules.Test
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	[TestFixture]
	public class RootMenuModelsTest
	{
		[Test]
		public void Should_return_status_ok_when_route_exists()
		{
		    // Given
		    var bootstrapper = new DefaultNancyBootstrapper();
		    var browser = new Browser(bootstrapper);
			
		    // When
		    var result = browser.Get("/", with => {
		        with.HttpRequest();
		    });
		
		    // Then
		    Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
		}
		
	}	
}