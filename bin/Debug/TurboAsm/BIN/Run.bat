mount C: "C:\Program Files (x86)\GUI Turbo Assembler\BIN"
            C:
            tasm.exe QJNQQDO.asm >gtasm.log
            tlink.exe /3 /x /v QJNQQDO >gtlink.log
            exit