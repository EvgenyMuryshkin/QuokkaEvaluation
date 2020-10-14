// partial declaration for Recursion SoC. Generated by QuokkaAssembly
using QuSoC;
namespace Recursion
{
	public partial class Recursion
	{
		internal SoCRegisterModule CounterModule;
		protected override ISoCComponentModule[] GeneratedModules => new ISoCComponentModule[] { CounterModule };
		protected override void CreateGeneratedModules()
		{
			CounterModule = new SoCRegisterModule();
		} // CreateGeneratedModules
		protected override void ScheduleGeneratedModules()
		{
			CounterModule.Schedule(() => new SoCRegisterModuleInputs() { Common = ModuleCommon, DeviceAddress = 0x80000000 });
		} // ScheduleGeneratedModules
	}
}