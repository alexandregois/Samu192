using UIKit;
using SAMU192iOS.FacadeStub;

namespace SAMU192iOS
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main (string[] args)
		{
            StubUtilidades.SetCultureInfo();
            StubCadastro.Carrega();
            StubConexao.Carrega();
            UIApplication.Main(args, null, "AppDelegate");
		}
    }
}
