using HealthPatient.Models;
using HealthPatient.ViewModels;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void IsValidPassword_ReturnsTrue()
        {
            var viewmodel = new CreateUserViewModel();
            string password = "Aa123!@#qfqF";
            bool result = viewmodel.IsPasswordValid(password);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidPassword_ReturnsFalse()
        {
            var viewmodel = new CreateUserViewModel();
            string password = "123";
            bool result = viewmodel.IsPasswordValid(password);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidPassword_PasswordIsNull_ReturnsFalse()
        {
            var viewmodel = new CreateUserViewModel();
            string password = null;
            bool result = viewmodel.IsPasswordValid(password);
            Assert.IsFalse(result);
        }
        
        [TestMethod]
        public void IsValidPassword_PasswordIsEmpty_ReturnsFalse()
        {
            var viewmodel = new CreateUserViewModel();
            string password = "";
            bool result = viewmodel.IsPasswordValid(password);
            Assert.IsFalse(result);
        }

        
    }
}