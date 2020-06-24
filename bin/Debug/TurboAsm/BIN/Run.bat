mount C: "C:\Program Files (x86)\GUI Turbo Assembler\BIN"
            C:
            tasm.exe EPJNEOX.asm >gtasm.log
            tlink.exe /3 /x /v EPJNEOX >gtlink.log
            exit