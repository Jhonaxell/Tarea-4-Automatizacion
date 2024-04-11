using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace Tarea4
{
    class Program
    {
        static void Main(string[] args)
        {
            // se crea el driver de selenium
            IWebDriver driver = new FirefoxDriver();

            // lista para almacenar los resultados
            List<string> testResults = new List<string>();

            // ejecutar pruebas y almacenar los resultados
            testResults.Add(NavegacionLibros(driver));
            testResults.Add(NavegacionAutores(driver));
            testResults.Add(FuncionalidadContacto(driver));
            testResults.Add(infoAutores(driver));
            testResults.Add(infoLibros(driver));

            // generacion del informe html
            GenerateHTMLReport(testResults);

            driver.Quit();
        }

        static string NavegacionLibros(IWebDriver driver)
        {
            //llamamos el link de la pagina a usar
            linkPagina(driver);
            //maximisamos la ventana del firefox
            driver.Manage().Window.Maximize();
            //ubicamos el link al que queremos acceder mediante el texto del link de la pagina
            IWebElement enlaceLibros = driver.FindElement(By.LinkText("Ver Libros"));

            // funcion para dar click
            enlaceLibros.Click();
            //validacion para saber si la navegacion fue exitosa
            string tituloEsperado = "Libros";
            string tituloActual = driver.Title;

            if (tituloActual == tituloEsperado)
            {
                //si fue exitosa entonces retornamos exitoso
                Console.WriteLine("\nLa navegación a la página de libros fue exitosa. ");
                capturasPantallas(driver, "NavegacionAutores");
                return "Exitoso";
            }
            else
            {
                //sino, enviamos fallido
                Console.WriteLine("\nError al ir a la página de libros. ");
                return "Fallido";

            }

        }

        static string NavegacionAutores(IWebDriver driver)
        {
            //llamamos el link de la pagina a usar
            linkPagina(driver);
            //ubicamos el link al que queremos acceder mediante el texto del link de la pagina
            IWebElement enlaceAutores = driver.FindElement(By.LinkText("Ver Autores"));
            enlaceAutores.Click();

            //validacion para saber si la navegacion fue exitosa
            string tituloEsperado = "Autores";
            string tituloActual = driver.Title;

            if (tituloActual == tituloEsperado)
            {
                Console.WriteLine("\nLa navegación a la página de Autores fue exitosa.");
                capturasPantallas(driver, "NavegacionAutores");
                return "Exitoso";
            }
            else
            {
                Console.WriteLine("\nError al ir a la página de Autores.");
                return "Fallido";
            }

        }

        static string FuncionalidadContacto(IWebDriver driver)
        {
            linkPagina(driver);
            //ubicamos el texto de contactanos
            IWebElement enlaceContacto = driver.FindElement(By.LinkText("Contactanos"));
            //le damos click
            enlaceContacto.Click();

            //aqui vemos el caso de prueba 1 de la funcionalidad de contactos
            Console.WriteLine("\nEjecutando Caso de Prueba 1: Envío Exitoso del Formulario de Contacto");
            string resultadoPrueba1 = CasoPrueba1(driver);

            Thread.Sleep(4000);

            //aqui vemos el caso de prueba 2 de la funcionalidad de contactos
            Console.WriteLine("\nEjecutando Caso de Prueba 2: Validación de Campos Obligatorios");
            string resultadoPrueba2 = CasoPrueba2(driver);

            Thread.Sleep(4000);

            //aqui vemos el caso de prueba 3 de la funcionalidad de contactos
            Console.WriteLine("\nEjecutando Caso de Prueba 3: Formato Válido de Correo Electrónico");
            string resultadoPrueba3 = CasoPrueba3(driver);

            Thread.Sleep(4000);

            //validacion de si dio resultado o no
            if (resultadoPrueba1 == "Exitoso" && resultadoPrueba2 == "Exitoso" && resultadoPrueba3 == "Exitoso")
            {
                return "Exitoso";
            }
            else
            {
                return "Fallido";
            }

        }
        static string CasoPrueba1(IWebDriver driver)
        {
            Thread.Sleep(2000);
            //aqui ubicamos lo que queremos llenar
            IWebElement nombreInput = driver.FindElement(By.Name("nombre"));
            IWebElement correoInput = driver.FindElement(By.Name("correo"));
            IWebElement asuntoInput = driver.FindElement(By.Name("asunto"));
            IWebElement mensajeInput = driver.FindElement(By.Name("mensaje"));
            IWebElement enviarButton = driver.FindElement(By.Name("Guardar"));
            //seleccionamos mensajes para poner en los imput 
            nombreInput.SendKeys("Nombre de Prueba");
            correoInput.SendKeys("correo@ejemplo.com");
            asuntoInput.SendKeys("Asunto de Prueba");
            mensajeInput.SendKeys("Mensaje de prueba para verificar la funcionalidad de contacto");
            //damos click para enviar
            enviarButton.Click();

            Thread.Sleep(4000);

            IWebElement mensajeConfirmacion = driver.FindElement(By.XPath("//div[@class='alert alert-success']"));
            if (mensajeConfirmacion.Text.Contains("Tu mensaje ha sido enviado con éxito."))
            {
                Console.WriteLine("Resultado Esperado: Se muestra un mensaje de confirmación indicando que el mensaje ha sido enviado correctamente. ");
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("window.scrollTo(0, 0);");
                capturasPantallas(driver, "CasoPrueba1");
                return "Exitoso";
            }
            else
            {
                Console.WriteLine("Error para caso de prueba 3. ");
                return "Fallido";
            }
            Thread.Sleep(3000);
        }

        static string CasoPrueba2(IWebDriver driver)
        {
            linkPagina(driver);

            IWebElement enlaceContacto = driver.FindElement(By.LinkText("Contactanos"));
            enlaceContacto.Click();
            Thread.Sleep(2000);

            IWebElement enviarButton = driver.FindElement(By.Name("Guardar"));

            enviarButton.Click();

            Thread.Sleep(2000);

            IWebElement mensajeError = driver.FindElement(By.XPath("//div[@class='alert alert-danger']"));
            if (mensajeError.Text.Contains("Los campos obligatorios deben ser completados."))
            {
                Console.WriteLine("Resultado Esperado: Se muestra un mensaje de error indicando que los campos obligatorios deben ser completados.");
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("window.scrollTo(0, 0);");
                capturasPantallas(driver, "CasoPrueba2");
                return "Exitoso";
            }
            else
            {
                Console.WriteLine("ERROR para caso de prueba 2. ");
                return "Fallido";
            }
            Thread.Sleep(3000);
        }

        static string CasoPrueba3(IWebDriver driver)
        {
            linkPagina(driver);

            IWebElement enlaceContacto = driver.FindElement(By.LinkText("Contactanos"));
            enlaceContacto.Click();
            Thread.Sleep(2000);

            IWebElement correoInput = driver.FindElement(By.Name("correo"));

            correoInput.SendKeys("pmjpd");

            IWebElement enviarButton = driver.FindElement(By.Name("Guardar"));
            enviarButton.Click();

            Thread.Sleep(4000);

            IWebElement mensajeError = driver.FindElement(By.XPath("//div[@class='alert alert-danger']"));
            if (mensajeError.Text.Contains("Los campos obligatorios deben ser completados."))
            {
                Console.WriteLine("Resultado Esperado: Se muestra un mensaje de error indicando que los campos obligatorios deben ser completados. ");
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("window.scrollTo(0, 0);");
                capturasPantallas(driver, "CasoPrueba3");
                return "Exitoso";
            }
            else
            {
                Console.WriteLine("ERROR para caso de prueba 3. ");
                return "Fallido";
            }
            Thread.Sleep(3000);
        }
        static string infoAutores(IWebDriver driver)
        {
            linkPagina(driver);

            IWebElement enlaceAutores = driver.FindElement(By.LinkText("Ver Autores"));
            enlaceAutores.Click();

            string tituloEsperado = "Autores";
            string tituloActual = driver.Title;

            if (tituloActual == tituloEsperado)
            {
                Console.WriteLine("Éxito: Navegación a la página de Autores.");
            }
            else
            {
                Console.WriteLine("Error: La navegación a la página de Autores falló.");
            }

            IWebElement botonInformacion = driver.FindElement(By.XPath("//button[text()='Información']"));
            botonInformacion.Click();

            Thread.Sleep(2000);

            IList<IWebElement> modals = driver.FindElements(By.XPath("//div[@class='modal-content']"));
            if (modals.Count > 0)
            {
                Console.WriteLine("Éxito: El modal se mostró correctamente.");
                capturasPantallas(driver, "InfoAutores");
                return "Exitoso";
            }
            else
            {
                Console.WriteLine("Error: No se mostró el modal de autores.");
                return "Fallido";
            }
        }

        static string infoLibros(IWebDriver driver)
        {
            linkPagina(driver);

            IWebElement enlaceLibros = driver.FindElement(By.LinkText("Ver Libros"));
            enlaceLibros.Click();

            string tituloEsperado = "Libros";
            string tituloActual = driver.Title;

            if (tituloActual == tituloEsperado)
            {
                Console.WriteLine("Éxito: Navegación a la página de Libros.");
            }
            else
            {
                Console.WriteLine("Error: La navegación a la página de Libros falló.");
            }

            IWebElement botonInformacion = driver.FindElement(By.XPath("//button[text()='Ver Información']"));
            botonInformacion.Click();

            Thread.Sleep(2000);

            IList<IWebElement> modals = driver.FindElements(By.XPath("//div[@class='modal-content']"));
            if (modals.Count > 0)
            {
                Console.WriteLine("Éxito: El modal se mostró correctamente.");
                capturasPantallas(driver, "Infolibro");
                return "Exitoso";
            }
            else
            {
                Console.WriteLine("Error: No se mostró el modal de libros.");
                return "Fallido";
            }

        }

        static void GenerateHTMLReport(List<string> testResults)
        {
            string reportFilePath = "TestReport.html";

            using (StreamWriter writer = new StreamWriter(reportFilePath))
            {
                writer.WriteLine("<!DOCTYPE html>");
                writer.WriteLine("<html>");
                writer.WriteLine("<head>");
                writer.WriteLine("<title>Test Report</title>");
                writer.WriteLine("</head>");
                writer.WriteLine("<body>");
                writer.WriteLine("<h1>Test Report</h1>");
                writer.WriteLine("<ul>");

                foreach (string result in testResults)
                {
                    string status = result == "Exitoso" ? "Exitoso" : "Fallido";
                    writer.WriteLine($"<li>Resultado de la prueba: {result} ({status})</li>");
                }


                writer.WriteLine("</ul>");
                writer.WriteLine("</body>");
                writer.WriteLine("</html>");
            }

            Console.WriteLine($"Informe generado en: {reportFilePath}");
        }
        public static void capturasPantallas(IWebDriver driver, string screenshotName)
        {
            string screenshotDirectory = "C:\\screenshots";

            if (!Directory.Exists(screenshotDirectory))
            {
                Directory.CreateDirectory(screenshotDirectory);
            }

            try
            {
                ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;
                Screenshot screenshot = screenshotDriver.GetScreenshot();
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string screenshotPath = Path.Combine(screenshotDirectory, $"{screenshotName}_{timestamp}.png");
                screenshot.SaveAsFile(screenshotPath);

                Console.WriteLine($"Captura de pantalla guardada en: {screenshotPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar la captura de pantalla: {ex.Message}");
            }
        }
        static void linkPagina(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("http://bibliotecajhon.infinityfreeapp.com/index.php");

        }
    }
}