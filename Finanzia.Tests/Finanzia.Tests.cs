
//#define CP001
//#define CP002
//#define CP003
//#define CP004
//#define CP005
//#define CP006
//#define CP007
//#define CP008
//#define CP009
//#define CP010
//#define CP011
//#define CP012
//#define CP013
//#define CP014

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;
using Finanzia.Tests.Utils;

class Program
{
    static void Main()
    {
#if CP001
        Ejecutar_CP001();
#elif CP002
        Ejecutar_CP002();
#elif CP003
        Ejecutar_CP003();
#elif CP004
        Ejecutar_CP004();
#elif CP005
        Ejecutar_CP005();
#elif CP006
        Ejecutar_CP006();
#elif CP007
        Ejecutar_CP007();
#elif CP008
        Ejecutar_CP008();
#elif CP009
        Ejecutar_CP009();
#elif CP010
        Ejecutar_CP010();
#elif CP011
        Ejecutar_CP011();
#elif CP012
        Ejecutar_CP012();
#elif CP013
        Ejecutar_CP013();
#elif CP014
        Ejecutar_CP014();
#endif
    }

    static void Ejecutar_CP001()
    {
        IWebDriver driver = new ChromeDriver();
        driver.Manage().Window.Maximize();

        try
        {
            LoginHelper.IniciarSesion(driver);

            driver.Navigate().GoToUrl("http://localhost:5267/Cliente");
            Thread.Sleep(2000);
            CapturaHelper.GuardarCaptura(driver, "CP001_RegistroCliente", "CP001_inicio");

            driver.FindElement(By.Id("btnNuevo")).Click();
            Thread.Sleep(1000);

            string cedula = "402-2992895-3";
            string nombre = "Jonny";
            string apellido = "Mendez";
            string correo = "jonny.mendez@example.com";
            string telefono = "829-555-1244";

            driver.FindElement(By.Id("txtNroDocumento")).SendKeys(cedula);
            driver.FindElement(By.Id("txtNombre")).SendKeys(nombre);
            driver.FindElement(By.Id("txtApellido")).SendKeys(apellido);
            driver.FindElement(By.Id("txtCorreo")).SendKeys(correo);
            driver.FindElement(By.Id("txtTelefono")).SendKeys(telefono);

            CapturaHelper.GuardarCaptura(driver, "CP001_RegistroCliente", "CP001_formulario");

            driver.FindElement(By.Id("btnGuardar")).Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d =>
            {
                try { return d.FindElement(By.ClassName("swal2-popup")).Displayed; }
                catch (NoSuchElementException) { return false; }
            });

            string resultado = driver.FindElement(By.ClassName("swal2-title")).Text;

            if (resultado.Contains("Error"))
            {
                string detalle = driver.FindElement(By.ClassName("swal2-html-container")).Text;
                Console.WriteLine("❌ Registro fallido: " + detalle);
            }
            else
            {
                Console.WriteLine("✅ Cliente registrado exitosamente.");
            }

            CapturaHelper.GuardarCaptura(driver, "CP001_RegistroCliente", "CP001_confirmacion");
        }
        catch (Exception ex)
        {
            Console.WriteLine("⚠️ Excepción durante la prueba: " + ex.Message);
        }
        finally
        {
            Thread.Sleep(3000);
            driver.Quit();
        }
    }
    static void Ejecutar_CP002()
    {
        IWebDriver driver = new ChromeDriver();
        driver.Manage().Window.Maximize();

        try
        {
            LoginHelper.IniciarSesion(driver);

            driver.Navigate().GoToUrl("http://localhost:5267/Cliente");
            Thread.Sleep(2000);
            CapturaHelper.GuardarCaptura(driver, "CP002_ValidacionCampos", "CP002_inicio");

            driver.FindElement(By.Id("btnNuevo")).Click();
            Thread.Sleep(1000);
            CapturaHelper.GuardarCaptura(driver, "CP002_ValidacionCampos", "CP002_modal_vacio");

            driver.FindElement(By.Id("btnGuardar")).Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d =>
            {
                try { return d.FindElement(By.ClassName("swal2-popup")).Displayed; }
                catch (NoSuchElementException) { return false; }
            });

            CapturaHelper.GuardarCaptura(driver, "CP002_ValidacionCampos", "CP002_alerta");

            string titulo = driver.FindElement(By.ClassName("swal2-title")).Text;
            string mensaje = driver.FindElement(By.ClassName("swal2-html-container")).Text;

            if (titulo.Contains("Error") || mensaje.Contains("formato"))
            {
                Console.WriteLine("✅ Validación de campos vacíos correcta: " + mensaje);
            }
            else
            {
                Console.WriteLine("❌ No se mostró la validación esperada.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("⚠️ Excepción durante la prueba: " + ex.Message);
        }
        finally
        {
            Thread.Sleep(3000);
            driver.Quit();
        }
    }
    static void Ejecutar_CP003()
    {
        IWebDriver driver = new ChromeDriver();
        driver.Manage().Window.Maximize();

        try
        {
            LoginHelper.IniciarSesion(driver);

            driver.Navigate().GoToUrl("http://localhost:5267/Cliente");
            Thread.Sleep(2000);
            CapturaHelper.GuardarCaptura(driver, "CP003_ValidacionCedulaDuplicada", "CP003_inicio");

            driver.FindElement(By.Id("btnNuevo")).Click();
            Thread.Sleep(1000);

            string cedulaDuplicada = "402-2992895-3";
            driver.FindElement(By.Id("txtNroDocumento")).SendKeys(cedulaDuplicada);
            driver.FindElement(By.Id("txtNombre")).SendKeys("Pedro");
            driver.FindElement(By.Id("txtApellido")).SendKeys("Reyes");
            driver.FindElement(By.Id("txtCorreo")).SendKeys("pedro.reyes@example.com");
            driver.FindElement(By.Id("txtTelefono")).SendKeys("829-555-9876");

            CapturaHelper.GuardarCaptura(driver, "CP003_ValidacionCedulaDuplicada", "CP003_formulario");

            driver.FindElement(By.Id("btnGuardar")).Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d =>
            {
                try { return d.FindElement(By.ClassName("swal2-popup")).Displayed; }
                catch (NoSuchElementException) { return false; }
            });

            CapturaHelper.GuardarCaptura(driver, "CP003_ValidacionCedulaDuplicada", "CP003_error_duplicado");

            string titulo = driver.FindElement(By.ClassName("swal2-title")).Text;
            string mensaje = driver.FindElement(By.ClassName("swal2-html-container")).Text;

            if (mensaje.Contains("ya existe"))
            {
                Console.WriteLine("✅ Validación de cédula duplicada correcta: " + mensaje);
            }
            else
            {
                Console.WriteLine("❌ No se mostró el mensaje de cédula duplicada.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("⚠️ Excepción durante la prueba: " + ex.Message);
        }
        finally
        {
            Thread.Sleep(3000);
            driver.Quit();
        }
    }
    static void Ejecutar_CP004()
    {
        IWebDriver driver = new ChromeDriver();
        driver.Manage().Window.Maximize();

        try
        {
            LoginHelper.IniciarSesion(driver);

            driver.Navigate().GoToUrl("http://localhost:5267/Prestamo/Nuevo");
            Thread.Sleep(2000);
            CapturaHelper.GuardarCaptura(driver, "CP004_CrearPrestamo", "CP004_inicio");

            driver.FindElement(By.Id("txtNroDocumento")).SendKeys("402-2992895-3");
            driver.FindElement(By.Id("btnBuscar")).Click();
            Thread.Sleep(1500);

            driver.FindElement(By.Id("txtMontoPrestamo")).SendKeys("10000");
            driver.FindElement(By.Id("txtInteres")).SendKeys("10");
            driver.FindElement(By.Id("txtNroCuotas")).SendKeys("5");

            var formaPago = new SelectElement(driver.FindElement(By.Id("cboFormaPago")));
            formaPago.SelectByText("Mensual");

            var moneda = new SelectElement(driver.FindElement(By.Id("cboTipoMoneda")));
            moneda.SelectByIndex(0);

            var fechaInicio = driver.FindElement(By.Id("txtFechaInicio"));
            fechaInicio.Clear();
            fechaInicio.SendKeys(DateTime.Now.AddDays(1).ToString("MM-dd-yyyy"));

            driver.FindElement(By.Id("btnCalcular")).Click();
            Thread.Sleep(1000);

            CapturaHelper.GuardarCaptura(driver, "CP004_CrearPrestamo", "CP004_formulario");

            driver.FindElement(By.Id("btnRegistrar")).Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d =>
            {
                try { return d.FindElement(By.ClassName("swal2-popup")).Displayed; }
                catch (NoSuchElementException) { return false; }
            });

            CapturaHelper.GuardarCaptura(driver, "CP004_CrearPrestamo", "CP004_confirmacion");

            string mensaje = driver.FindElement(By.ClassName("swal2-html-container")).Text;
            if (mensaje.Contains("registrado exitosamente"))
            {
                Console.WriteLine("✅ Préstamo registrado con éxito.");
            }
            else
            {
                Console.WriteLine("❌ No se mostró mensaje de confirmación esperado.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("⚠️ Error durante la prueba: " + ex.Message);
        }
        finally
        {
            Thread.Sleep(3000);
            driver.Quit();
        }
    }
    static void Ejecutar_CP005()
    {
        IWebDriver driver = new ChromeDriver();
        driver.Manage().Window.Maximize();

        try
        {
            LoginHelper.IniciarSesion(driver);
            driver.Navigate().GoToUrl("http://localhost:5267/Cliente");
            Thread.Sleep(2000);

            CapturaHelper.GuardarCaptura(driver, "CP005_EliminarCliente", "CP005_inicio");

            IWebElement tabla = driver.FindElement(By.Id("tbData"));
            IWebElement primeraFilaEliminar = tabla.FindElement(By.CssSelector("tbody tr:first-child .btn-eliminar"));
            primeraFilaEliminar.Click();

            Thread.Sleep(1000);

            IWebElement confirmar = driver.FindElement(By.CssSelector(".swal2-confirm"));
            confirmar.Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d =>
            {
                try
                {
                    return d.FindElement(By.ClassName("swal2-title")).Text.Contains("Listo") ||
                           d.FindElement(By.ClassName("swal2-title")).Text.Contains("Error");
                }
                catch
                {
                    return false;
                }
            });

            CapturaHelper.GuardarCaptura(driver, "CP005_EliminarCliente", "CP005_resultado");

            string mensaje = driver.FindElement(By.ClassName("swal2-html-container")).Text;
            Console.WriteLine($"📢 Resultado de eliminación: {mensaje}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("⚠️ Error durante la prueba CP005: " + ex.Message);
        }
        finally
        {
            Thread.Sleep(3000);
            driver.Quit();
        }
    }
    static void Ejecutar_CP006()
    {
        IWebDriver driver = new ChromeDriver();
        driver.Manage().Window.Maximize();

        try
        {
            LoginHelper.IniciarSesion(driver);
            driver.Navigate().GoToUrl("http://localhost:5267/Prestamo/Nuevo");
            Thread.Sleep(2000);
            CapturaHelper.GuardarCaptura(driver, "CP006_ClienteInexistente", "CP006_inicio");

            driver.FindElement(By.Id("txtNroDocumento")).SendKeys("999-9999999-9");
            driver.FindElement(By.Id("btnBuscar")).Click();
            Thread.Sleep(1000);

            CapturaHelper.GuardarCaptura(driver, "CP006_ClienteInexistente", "CP006_busquedaCliente");

            string mensaje = driver.FindElement(By.ClassName("swal2-html-container")).Text;
            if (mensaje.Contains("no existe"))
            {
                Console.WriteLine("✅ Error esperado mostrado correctamente: " + mensaje);
            }
            else
            {
                Console.WriteLine("❌ No se mostró el error esperado.");
            }

            CapturaHelper.GuardarCaptura(driver, "CP006_ClienteInexistente", "CP006_mensaje");
        }
        catch (Exception ex)
        {
            Console.WriteLine("⚠️ Error durante la prueba CP006: " + ex.Message);
        }
        finally
        {
            Thread.Sleep(3000);
            driver.Quit();
        }
    }
    static void Ejecutar_CP007()
    {
        IWebDriver driver = new ChromeDriver();
        driver.Manage().Window.Maximize();

        try
        {
            LoginHelper.IniciarSesion(driver);
            driver.Navigate().GoToUrl("http://localhost:5267/Prestamo/Nuevo");
            Thread.Sleep(1500);

            string documentoCliente = "402-2992895-3";

            driver.FindElement(By.Id("txtNroDocumento")).SendKeys(documentoCliente);
            driver.FindElement(By.Id("btnBuscar")).Click();
            Thread.Sleep(1500);

            var campos = new[]
            {
                "txtMontoPrestamo",
                "txtInteres",
                "txtNroCuotas",
                "cboFormaPago",
                "cboTipoMoneda",
                "txtFechaInicio"
            };

            string[] nombres = {
                "Monto Prestamo",
                "Interés",
                "Número de Cuotas",
                "Forma de Pago",
                "Moneda",
                "Fecha de Inicio"
            };

            for (int i = 0; i < campos.Length; i++)
            {
                driver.Navigate().GoToUrl("http://localhost:5267/Prestamo/Nuevo");
                Thread.Sleep(1500);

                driver.FindElement(By.Id("txtNroDocumento")).SendKeys(documentoCliente);
                driver.FindElement(By.Id("btnBuscar")).Click();
                Thread.Sleep(1000);

                if (i != 0) driver.FindElement(By.Id("txtMontoPrestamo")).SendKeys("10000");
                if (i != 1) driver.FindElement(By.Id("txtInteres")).SendKeys("10");
                if (i != 2) driver.FindElement(By.Id("txtNroCuotas")).SendKeys("5");
                if (i != 3)
                {
                    var formaPago = new SelectElement(driver.FindElement(By.Id("cboFormaPago")));
                    formaPago.SelectByText("Mensual");
                }
                if (i != 4)
                {
                    var moneda = new SelectElement(driver.FindElement(By.Id("cboTipoMoneda")));
                    moneda.SelectByIndex(0);
                }

                if (i != 5) driver.FindElement(By.Id("txtFechaInicio")).SendKeys(DateTime.Now.AddDays(1).ToString("MM/dd/yyyy"));

                driver.FindElement(By.Id("btnRegistrar")).Click();

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(d =>
                {
                    try { return d.FindElement(By.ClassName("swal2-popup")).Displayed; }
                    catch (NoSuchElementException) { return false; }
                });

                Thread.Sleep(500); 
                string mensaje = driver.FindElement(By.ClassName("swal2-html-container")).Text;
                string nombreCampo = nombres[i];
                if (mensaje.ToLower().Contains("campo") || mensaje.ToLower().Contains("completo") || mensaje.ToLower().Contains("llenar"))
                {
                    Console.WriteLine($"✅ Validación correcta al omitir el campo '{nombreCampo}': {mensaje}");
                }
                else
                {
                    Console.WriteLine($"❌ No se mostró mensaje esperado al omitir el campo '{nombreCampo}'");
                }

                CapturaHelper.GuardarCaptura(driver, "CP007_ValidacionCamposPrestamo", $"CP007_falta_{nombreCampo.Replace(" ", "")}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("⚠️ Error durante la prueba CP007: " + ex.Message);
        }
        finally
        {
            Thread.Sleep(3000);
            driver.Quit();
        }
    }
    static void Ejecutar_CP008()
    {
        IWebDriver driver = new ChromeDriver();
        driver.Manage().Window.Maximize();

        try
        {
            LoginHelper.IniciarSesion(driver);
            driver.Navigate().GoToUrl("http://localhost:5267/Cobrar");
            Thread.Sleep(1500);

            string[] cedulasInvalidas = {
            "123456789",                      
            "000-0000000-0",                  
            "666-7788990-8"                    
        };

            string[] motivos = {
            "formato inválido",
            "cliente no existe",
            "cliente sin préstamos"
        };

            for (int i = 0; i < cedulasInvalidas.Length; i++)
            {
                driver.Navigate().GoToUrl("http://localhost:5267/Cobrar");
                Thread.Sleep(1000);

                driver.FindElement(By.Id("txtNroDocumento")).Clear();
                driver.FindElement(By.Id("txtNroDocumento")).SendKeys(cedulasInvalidas[i]);
                driver.FindElement(By.Id("btnBuscar")).Click();

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(d =>
                {
                    try { return d.FindElement(By.ClassName("swal2-popup")).Displayed; }
                    catch { return false; }
                });

                string mensaje = driver.FindElement(By.ClassName("swal2-html-container")).Text;
                Console.WriteLine($"🧪 Validación de {motivos[i]}: {mensaje}");
                Thread.Sleep(1500);
                CapturaHelper.GuardarCaptura(driver, "CP008_ValidacionesPagos", $"CP008_error_{motivos[i].Replace(" ", "_")}");
            }

            driver.Navigate().GoToUrl("http://localhost:5267/Cobrar");
            Thread.Sleep(1000);

            driver.FindElement(By.Id("txtNroDocumento")).Clear();
            driver.FindElement(By.Id("txtNroDocumento")).SendKeys("402-2992895-3"); 
            driver.FindElement(By.Id("btnBuscar")).Click();
            Thread.Sleep(2000);

            CapturaHelper.GuardarCaptura(driver, "CP008_ValidacionesPagos", "CP008_cliente_con_prestamo");

            driver.FindElement(By.Id("btnRegistrarPago")).Click();

            WebDriverWait waitNoCuotas = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            waitNoCuotas.Until(d =>
            {
                try { return d.FindElement(By.ClassName("swal2-popup")).Displayed; }
                catch { return false; }
            });

            string mensajeCuotas = driver.FindElement(By.ClassName("swal2-html-container")).Text;
            Console.WriteLine($"🧪 Validación sin cuotas seleccionadas: {mensajeCuotas}");
            Thread.Sleep(1500);
            CapturaHelper.GuardarCaptura(driver, "CP008_ValidacionesPagos", "CP008_error_no_cuotas");
        }
        catch (Exception ex)
        {
            Console.WriteLine("⚠️ Error durante la prueba CP008: " + ex.Message);
        }
        finally
        {
            Thread.Sleep(3000);
            driver.Quit();
        }
    }
    static void Ejecutar_CP009()
    {
        IWebDriver driver = new ChromeDriver();
        driver.Manage().Window.Maximize();

        try
        {
            LoginHelper.IniciarSesion(driver);
            driver.Navigate().GoToUrl("http://localhost:5267/Cobrar");
            Thread.Sleep(1500);

            string documento = "402-2992895-3";

            driver.FindElement(By.Id("txtNroDocumento")).SendKeys(documento);
            driver.FindElement(By.Id("btnBuscar")).Click();
            Thread.Sleep(2000);

            CapturaHelper.GuardarCaptura(driver, "CP009_PagoExitoso", "CP009_inicio");

            var checkboxes = driver.FindElements(By.ClassName("checkPagado"));
            if (checkboxes.Count == 0)
            {
                Console.WriteLine("⚠️ No hay cuotas pendientes para este cliente.");
                return;
            }

            checkboxes[0].Click(); 
            Thread.Sleep(1000);

            CapturaHelper.GuardarCaptura(driver, "CP009_PagoExitoso", "CP009_cuota_seleccionada");

            driver.FindElement(By.Id("btnRegistrarPago")).Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d =>
            {
                try { return d.FindElement(By.ClassName("swal2-popup")).Displayed; }
                catch { return false; }
            });

            Thread.Sleep(1000);
            CapturaHelper.GuardarCaptura(driver, "CP009_PagoExitoso", "CP009_confirmacion");

            string mensaje = driver.FindElement(By.ClassName("swal2-html-container")).Text;
            if (mensaje.ToLower().Contains("pago registrado"))
            {
                Console.WriteLine("✅ Pago registrado exitosamente.");
            }
            else
            {
                Console.WriteLine("❌ No se mostró el mensaje de éxito esperado.");
            }

            driver.Navigate().GoToUrl("http://localhost:5267/Cobrar");
            Thread.Sleep(1500);

            driver.FindElement(By.Id("txtNroDocumento")).SendKeys(documento);
            driver.FindElement(By.Id("btnBuscar")).Click();
            Thread.Sleep(2000);

            CapturaHelper.GuardarCaptura(driver, "CP009_PagoExitoso", "CP009_verificacion_estado_final");

            Console.WriteLine("📸 Se volvió a cargar el estado del préstamo para verificar visualmente el pago.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("⚠️ Error durante la prueba CP009: " + ex.Message);
        }
        finally
        {
            Thread.Sleep(3000);
            driver.Quit();
        }
    }
    static void Ejecutar_CP010()
    {
        IWebDriver driver = new ChromeDriver();
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        driver.Manage().Window.Maximize();

        try
        {
            LoginHelper.IniciarSesion(driver);
            driver.Navigate().GoToUrl("http://localhost:5267/Moneda");
            Thread.Sleep(1500);

            CapturaHelper.GuardarCaptura(driver, "CP010_Moneda", "CP010_inicio");

            driver.FindElement(By.Id("btnNuevo")).Click();
            wait.Until(d => d.FindElement(By.Id("txtNombre")).Displayed);
            driver.FindElement(By.Id("txtNombre")).SendKeys("Peso Mexicano");
            driver.FindElement(By.Id("txtSimbolo")).SendKeys("MXN");
            CapturaHelper.GuardarCaptura(driver, "CP010_Moneda", "CP010_formulario_crear");
            driver.FindElement(By.Id("btnGuardar")).Click();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.ClassName("swal2-popup")));
            CapturaHelper.GuardarCaptura(driver, "CP010_Moneda", "CP010_confirmacion_crear");
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.ClassName("swal2-confirm"))).Click();

            Thread.Sleep(1500);
            driver.FindElement(By.Id("btnNuevo")).Click();
            wait.Until(d => d.FindElement(By.Id("txtNombre")).Displayed);
            driver.FindElement(By.Id("txtNombre")).SendKeys("Peso Mexicano");
            driver.FindElement(By.Id("txtSimbolo")).SendKeys("MXN");
            driver.FindElement(By.Id("btnGuardar")).Click();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.ClassName("swal2-popup")));
            CapturaHelper.GuardarCaptura(driver, "CP010_Moneda", "CP010_intento_duplicado");
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.ClassName("swal2-confirm"))).Click();

            driver.FindElement(By.CssSelector(".btn-close")).Click();
            Thread.Sleep(1000); 

            Thread.Sleep(1500);
            var editarBtn = driver.FindElement(By.CssSelector(".btn-editar"));
            editarBtn.Click();
            wait.Until(d => d.FindElement(By.Id("txtNombre")).Displayed);

            var txtNombre = driver.FindElement(By.Id("txtNombre"));
            txtNombre.Clear();
            txtNombre.SendKeys("Peso MX");
            CapturaHelper.GuardarCaptura(driver, "CP010_Moneda", "CP010_formulario_editar");
            driver.FindElement(By.Id("btnGuardar")).Click();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.ClassName("swal2-popup")));
            CapturaHelper.GuardarCaptura(driver, "CP010_Moneda", "CP010_confirmacion_editar");
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.ClassName("swal2-confirm"))).Click();

            Thread.Sleep(1500);
            var eliminarBtn = driver.FindElement(By.CssSelector(".btn-eliminar"));
            eliminarBtn.Click();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.ClassName("swal2-popup")));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.ClassName("swal2-confirm"))).Click();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.ClassName("swal2-popup")));
            CapturaHelper.GuardarCaptura(driver, "CP010_Moneda", "CP010_confirmacion_eliminar");
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.ClassName("swal2-confirm"))).Click();

            Console.WriteLine("✅ CP010 completado correctamente");
        }
        catch (Exception ex)
        {
            Console.WriteLine("⚠️ Error en CP010: " + ex.Message);
        }
        finally
        {
            Thread.Sleep(3000);
            driver.Quit();
        }
    }
    static void Ejecutar_CP011()
    {
        IWebDriver driver = new ChromeDriver();
        driver.Manage().Window.Maximize();
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        try
        {
            LoginHelper.IniciarSesion(driver);
            driver.Navigate().GoToUrl("http://localhost:5267/Prestamo");
            Thread.Sleep(2000);
            CapturaHelper.GuardarCaptura(driver, "CP011_ImprimirPrestamo", "CP011_inicio");

            wait.Until(d => d.FindElement(By.CssSelector(".btn-detalle"))).Click();
            Thread.Sleep(1000);
            CapturaHelper.GuardarCaptura(driver, "CP011_ImprimirPrestamo", "CP011_detalle_prestamo");

            wait.Until(d => d.FindElement(By.Id("btnImprimir")).Displayed);
            driver.FindElement(By.Id("btnImprimir")).Click();
            Thread.Sleep(3000);
            CapturaHelper.GuardarCaptura(driver, "CP011_ImprimirPrestamo", "CP011_pdf_generado");

            Console.WriteLine("✅ CP011 completado correctamente");
        }
        catch (Exception ex)
        {
            Console.WriteLine("⚠️ Error en CP011: " + ex.Message);
        }
        finally
        {
            Thread.Sleep(3000);
            driver.Quit();
        }
    }
    static void Ejecutar_CP012()
    {
        IWebDriver driver = new ChromeDriver();
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        driver.Manage().Window.Maximize();

        try
        {
            LoginHelper.IniciarSesion(driver);
            driver.Navigate().GoToUrl("http://localhost:5267/Cobrar");
            Thread.Sleep(2000);
            CapturaHelper.GuardarCaptura(driver, "CP012_Pago", "CP012_inicio");

            driver.FindElement(By.Id("txtNroDocumento")).SendKeys("402-2992895-3");
            driver.FindElement(By.Id("btnBuscar")).Click();

            wait.Until(d => d.FindElements(By.CssSelector(".checkPagado")).Count > 0);
            Thread.Sleep(1000);
            CapturaHelper.GuardarCaptura(driver, "CP012_Pago", "CP012_cuotas_cargadas");

            var checkboxes = driver.FindElements(By.CssSelector(".checkPagado"));
            foreach (var checkbox in checkboxes)
            {
                if (!checkbox.Selected && checkbox.Enabled)
                    checkbox.Click();
            }

            Thread.Sleep(1000);
            CapturaHelper.GuardarCaptura(driver, "CP012_Pago", "CP012_todas_seleccionadas");

            driver.FindElement(By.Id("btnRegistrarPago")).Click();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.ClassName("swal2-popup")));
            Thread.Sleep(1000);
            CapturaHelper.GuardarCaptura(driver, "CP012_Pago", "CP012_confirmacion");

            Console.WriteLine("✅ CP012 completada exitosamente.");
        }
        catch (Exception ex)
        {
            Thread.Sleep(1000);
            CapturaHelper.GuardarCaptura(driver, "CP012_Pago", "CP012_error");
            Console.WriteLine($"❌ Error en CP012: {ex.Message}");
        }
        finally
        {
            driver.Quit();
        }
    }
    static void Ejecutar_CP013()
    {
        IWebDriver driver = new ChromeDriver();
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        driver.Manage().Window.Maximize();

        try
        {
            LoginHelper.IniciarSesion(driver);
            driver.Navigate().GoToUrl("http://localhost:5267/Cliente");
            Thread.Sleep(2000);
            CapturaHelper.GuardarCaptura(driver, "CP013", "01_inicio");

            string documentoConHistorial = "402-2992895-3";
            driver.FindElement(By.CssSelector("input[type='search']")).SendKeys(documentoConHistorial);
            Thread.Sleep(2000);
            CapturaHelper.GuardarCaptura(driver, "CP013", "02_cliente_filtrado");

            driver.FindElement(By.CssSelector("button.btn-danger")).Click();
            Thread.Sleep(1000);
            CapturaHelper.GuardarCaptura(driver, "CP013", "03_confirmacion_eliminar_abierta");

            driver.FindElement(By.XPath("//button[contains(text(),'Sí, continuar')]")).Click();
            Thread.Sleep(2000);
            CapturaHelper.GuardarCaptura(driver, "CP013", "04_confirmacion_eliminar_click");

            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("swal2-popup")));
            CapturaHelper.GuardarCaptura(driver, "CP013", "05_mensaje_confirmacion");

            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            CapturaHelper.GuardarCaptura(driver, "CP013", "06_mensaje_cerrado");

            string documentoSinHistorial = "666-7788990-8";
            driver.FindElement(By.CssSelector("input[type='search']")).Clear();
            driver.FindElement(By.CssSelector("input[type='search']")).SendKeys(documentoSinHistorial);
            Thread.Sleep(2000);
            CapturaHelper.GuardarCaptura(driver, "CP013", "07_cliente_sin_historial_filtrado");

            driver.FindElement(By.CssSelector("button.btn-danger")).Click();
            Thread.Sleep(1000);
            CapturaHelper.GuardarCaptura(driver, "CP013", "08_confirmacion_eliminar_cliente_sin_historial");

            driver.FindElement(By.XPath("//button[contains(text(),'Sí, continuar')]")).Click();
            Thread.Sleep(2000);
            CapturaHelper.GuardarCaptura(driver, "CP013", "09_eliminado_correctamente");

            driver.FindElement(By.CssSelector("input[type='search']")).Clear();
            driver.FindElement(By.CssSelector("input[type='search']")).SendKeys(documentoSinHistorial);
            Thread.Sleep(1500);
            CapturaHelper.GuardarCaptura(driver, "CP013", "10_confirmacion_ausencia_cliente");
        }
        catch (Exception ex)
        {
            CapturaHelper.GuardarCaptura(driver, "CP013", "ERROR");
            Console.WriteLine("Error en CP013: " + ex.Message);
        }
        finally
        {
            driver.Quit();
        }
    }
    static void Ejecutar_CP014()
    {
        Console.WriteLine("CP014 - Visualización del dashboard de resumen");

        IWebDriver driver = new ChromeDriver();
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        driver.Navigate().GoToUrl("http://localhost:5267/Home");
        driver.Manage().Window.Maximize();

        try
        {
            LoginHelper.IniciarSesion(driver);
            Thread.Sleep(2000);

            CapturaHelper.GuardarCaptura(driver, "CP014_Resumen", "CP014_01_inicio");

            string[] cardIds = { "spTotalClientes", "spPrestamosPendientes", "spPrestamosCancelados", "spInteresAcumulado" };
            foreach (var id in cardIds)
            {
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id)));
                IWebElement card = driver.FindElement(By.Id(id));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", card);
                Thread.Sleep(500);
                if (!card.Displayed)
                    throw new Exception($"No se muestra la tarjeta con ID: {id}");
            }
            CapturaHelper.GuardarCaptura(driver, "CP014_Resumen", "CP014_02_tarjetas");

            string[] chartIds = { "chartPrestamos", "chartIngresos" };
            foreach (var id in chartIds)
            {
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id)));
                IWebElement chart = driver.FindElement(By.Id(id));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", chart);
                Thread.Sleep(500);
                if (!chart.Displayed)
                    throw new Exception($"No se muestra la gráfica con ID: {id}");
            }
            CapturaHelper.GuardarCaptura(driver, "CP014_Resumen", "CP014_03_graficas");

            string[] tableIds = { "tablaPagosProximos", "tablaPagosAtrasados" };
            foreach (var id in tableIds)
            {
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id)));
                IWebElement table = driver.FindElement(By.Id(id));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", table);
                Thread.Sleep(500);
                if (!table.Displayed)
                    throw new Exception($"No se muestra la tabla con ID: {id}");
            }
            CapturaHelper.GuardarCaptura(driver, "CP014_Resumen", "CP014_04_tablas");

            Console.WriteLine("✅ CP014 completado correctamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ CP014 falló: {ex.Message}");
            CapturaHelper.GuardarCaptura(driver, "CP014_Resumen", "CP014_Error");
        }
        finally
        {
            driver.Quit();
        }
    }

}
