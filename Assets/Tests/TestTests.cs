using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

namespace LCPS.SlipForge
{
    /// <summary>
    /// Just a simple test class to assert that tests are working.
    /// </summary>
    public class TestTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestTestsSimplePasses()
        {
            // Use the Assert class to test conditions
            Assert.IsTrue(true);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator TestTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;

            Assert.IsTrue(true);
        }
    }
}
