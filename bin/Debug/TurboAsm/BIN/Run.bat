mount C: "C:\Program Files (x86)\GUI Turbo Assembler\BIN"
            C:
            tasm.exe GNFWEVO.asm >gtasm.log
            tlink.exe /3 /x /v GNFWEVO >gtlink.log
            exit