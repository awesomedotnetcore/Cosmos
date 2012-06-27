using System;
using System.Linq;
using Cosmos.Assembler;
using Cosmos.Assembler.x86;

namespace Cosmos.Debug.DebugStub {
	public class Init : Cosmos.Assembler.Code {
		public override void Assemble() {
			new Comment("X#: Group DebugStub");

			new Comment("Called before Kernel runs. Inits debug stub, etc");

			new Comment("X#: procedure Init {");
			new Label("DebugStub_Init");

			new Comment("X#: Call .Cls");
			new Call { DestinationLabel = "DebugStub_Cls" };

			new Comment("Display message before even trying to init serial");

			new Comment("X#: Call .DisplayWaitMsg");
			new Call { DestinationLabel = "DebugStub_DisplayWaitMsg" };

			new Comment("X#: Call .InitSerial");
			new Call { DestinationLabel = "DebugStub_InitSerial" };

			new Comment("X#: Call .WaitForDbgHandshake");
			new Call { DestinationLabel = "DebugStub_WaitForDbgHandshake" };

			new Comment("X#: Call .Cls");
			new Call { DestinationLabel = "DebugStub_Cls" };

			new Comment("X#: }");
			new Label("DebugStub_Init_Exit");
			new Return();

			new Comment("X#: procedure WaitForSignatureNew {");
			new Label("DebugStub_WaitForSignatureNew");

			new Comment("X#: EBX = 0");
			new Mov{ DestinationReg = RegistersEnum.EBX, SourceValue = 0 };

			new Comment("X#: Read:");
			new Label("DebugStub_WaitForSignatureNew_Read");

			new Comment("X#: Call .ReadALFromComPort");
			new Call { DestinationLabel = "DebugStub_ReadALFromComPort" };

			new Comment("X#: BL = AL");
			new Mov{ DestinationReg = RegistersEnum.BL, SourceReg = RegistersEnum.AL };

			new Comment("EBX.RotateRight(8)");

			new Comment("EBX.Compare(Cosmos.Debug.Consts.Consts.SerialSignature)");

			new Comment("JumpIf(Flags.NotEqual, \"DebugStub_WaitForSignature_Read\")");

			new Comment("X#: }");
			new Label("DebugStub_WaitForSignatureNew_Exit");
			new Return();

			new Comment("QEMU (and possibly others) send some garbage across the serial line first.");

			new Comment("Actually they send the garbage inbound, but garbage could be inbound as well so we");

			new Comment("keep this.");

			new Comment("To work around this we send a signature. DC then discards everything before the signature.");

			new Comment("QEMU has other serial issues too, and we dont support it anymore, but this signature is a good");

			new Comment("feature so we kept it.");

			new Comment("X#: procedure WaitForDbgHandshakeNew {");
			new Label("DebugStub_WaitForDbgHandshakeNew");

			new Comment("\"Clear\" the UART out");

			new Comment("X#: AL = 0");
			new Mov{ DestinationReg = RegistersEnum.AL, SourceValue = 0 };

			new Comment("X#: Call .WriteALToComPort");
			new Call { DestinationLabel = "DebugStub_WriteALToComPort" };

			new Comment("Cosmos.Debug.Consts.Consts.SerialSignature");

			new Comment("X#: +$19740807");
			new Push { DestinationValue = 0x19740807, Size = 32 };

			new Comment("X#: ESI = ESP");
			new Mov{ DestinationReg = RegistersEnum.ESI, SourceReg = RegistersEnum.ESP };

			new Comment("TODO pass a count register");

			new Comment("X#: Call .WriteByteToComPort");
			new Call { DestinationLabel = "DebugStub_WriteByteToComPort" };

			new Comment("X#: Call .WriteByteToComPort");
			new Call { DestinationLabel = "DebugStub_WriteByteToComPort" };

			new Comment("X#: Call .WriteByteToComPort");
			new Call { DestinationLabel = "DebugStub_WriteByteToComPort" };

			new Comment("X#: Call .WriteByteToComPort");
			new Call { DestinationLabel = "DebugStub_WriteByteToComPort" };

			new Comment("Restore ESP, we actually dont care about EAX or the value on the stack anymore.");

			new Comment("X#: -EAX");
			new Pop { DestinationReg = RegistersEnum.EAX};

			new Comment("We could use the signature as the start signal, but I prefer");

			new Comment("to keep the logic separate, especially in DC.");

			new Comment("Send the actual started signal");

			new Comment("DsVsip.Started = 6");

			new Comment("X#: AL = 6");
			new Mov{ DestinationReg = RegistersEnum.AL, SourceValue = 6 };

			new Comment("X#: Call .WriteALToComPort");
			new Call { DestinationLabel = "DebugStub_WriteALToComPort" };

			new Comment("X#: Call .WaitForSignature");
			new Call { DestinationLabel = "DebugStub_WaitForSignature" };

			new Comment("X#: Call .ProcessCommandBatch");
			new Call { DestinationLabel = "DebugStub_ProcessCommandBatch" };

			new Comment("X#: }");
			new Label("DebugStub_WaitForDbgHandshakeNew_Exit");
			new Return();

		}
	}
}