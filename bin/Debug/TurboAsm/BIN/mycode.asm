;--------------[INICIO]--------------
;-----[DECLARACION DE MACROS]--------
;______________________MACRO_SALVAR_____________________ 
;Guarda el estado actual de los registros de proposito general en la direccion
;de tabla indicada por el desplazamiento registros_tbl
SalvaRegistros MACRO    ;Delcaracion de la Macro 'SalvaRegistros' con los parametros:
	MOV [registros_tbl],Ax  ;Asigna el contenido de Ax a [registros_tbl]
	MOV [registros_tbl+2h],Bx   ;Asigna el contenido de Bx a [registros_tbl+2h]
	MOV [registros_tbl+4h],Cx   ;Asigna el contenido de Cx a [registros_tbl+4h]
	MOV [registros_tbl+6h],Dx   ;Asigna el contenido de Dx a [registros_tbl+6h]
	endm    ;Fin de la Macro
;______________________MACRO_RECUPERAR_____________________ 
;Reestablece los registros de proposito general al ultimo estado 
;en el que se salvaron en la direccion de tabla indicada por el desplazamiento registros_tbl
RecuperaRegistros MACRO ;Delcaracion de la Macro 'RecuperaRegistros' con los parametros:
	MOV Ax,[registros_tbl]  ;Asigna el contenido de [registros_tbl] a Ax
	MOV Bx,[registros_tbl+2h]   ;Asigna el contenido de [registros_tbl+2h] a Bx
	MOV Cx,[registros_tbl+4h]   ;Asigna el contenido de [registros_tbl+4h] a Cx
	MOV Dx,[registros_tbl+6h]   ;Asigna el contenido de [registros_tbl+6h] a Dx
endm
DISPLAY MACRO MENSAJE   ;Delcaracion de la Macro 'DISPLAY' con los parametros: MENSAJE
	SalvaRegistros  ;Guarda el estado actual de los registros de proposito general
	mov ah,09h  ;asigna 9 a ah (imprime cadena)    
	LEA Dx,MENSAJE  ;asigna a Dx la direccion inicial desplazamiento de la variable 'MENSAJE'
	int 21h ;Interrupcion 21h (funciones de el DOS API)
	RecuperaRegistros   ;Reestablece los registros de proposito general al ultimo estado en el que se salvaron
ENDM
;_____________________PRINT CHAR___________________
PRINT_CHAR MACRO CARACTER,pX,pY ;Delcaracion de la Macro 'PRINT_CHAR' con los parametros: CARACTER,pX,pY
	SalvaRegistros  ;Guarda el estado actual de los registros de proposito general
    ;GOTOXY pX,pY  ;mueve el cursor a las coordenadas (Px,Py)
    ;este segmento es solo para emu en bolrand se elimina
	MOV Ah, 02h ;goto xy
	MOV Bh, 00  ;Asigna el contenido de 00 a Bh
	MOV dh, pY  ;posicion del cursor en y
	MOV dl, pX  ;posicion del cursor en x
	int 10h ;Interrupcion 10h (Servicios de Pantalla)
    ;fin solo para emu 
	MOV DL,CARACTER ;Asigna el contenido de CARACTER a DL
	MOV AH,02   ;Asigna 02 a AH
	INT 21H ;Interrupcion 21h (funciones de el DOS API)
	RecuperaRegistros   ;Reestablece los registros de proposito general al ultimo estado en el que se salvaron
ENDM
;______________________COLOR_______________________
COLOR MACRO ALTO,ANCHO,X,Y,C    ;Delcaracion de la Macro 'COLOR' con los parametros: ALTO,ANCHO,X,Y,C
	SalvaRegistros  ;Guarda el estado actual de los registros de proposito general

	MOV DX,0000h    ;Asigna 0000h a DX
	MOV CX,0000h    ;Asigna 0000h a CX
      
	MOV CL,X    ;posicion inicial en x
	MOV CH,Y    ;posicion inicial en y
      ;
	MOV AL,X    ;Asigna el contenido de X a AL
	ADD AL,ANCHO    ;Suma el valor de AL con el de ANCHO, el resultado queda en AL
	MOV DL,AL   ;posicion final en x
      ;
      
	MOV AL,Y    ;Asigna el contenido de Y a AL
	ADD AL,ALTO ;Suma el valor de AL con el de ALTO, el resultado queda en AL
	MOV DH,AL   ;posicion final en y
        
          
	MOV AH,06H  ;Asigna 06H a AH
	MOV AL,00H  ;Asigna 00H a AL
	MOV BH,C    ;color de fondo y letra despues de borrar
      
	INT 10H ;Interrupcion 10h (Servicios de Pantalla)
       
	RecuperaRegistros   ;Reestablece los registros de proposito general al ultimo estado en el que se salvaron
ENDM
;----------------------PAUSA-----------------------
PAUSA MACRO                                                                 	;Delcaracion de la Macro 'PAUSA' con los parametros:
	SalvaRegistros                                                              ;Guarda el estado actual de los registros de proposito general
	XOR AX,AX                                                                   ;Resituye el registro  AX a ceros por medio de XOR
	INT 16h                                                                     ;Interrupcion 16h (funciones de llamada al teclado)
	RecuperaRegistros                                                           ;Reestablece los registros de proposito general al ultimo estado en el que se salvaron
ENDM                                                                        	;Fin de la Macro
;______________________MACR_XY_____________________
GotoXY MACRO gx,gy                                                          	;Delcaracion de la Macro 'GotoXY' con los parametros: gx,gy
	SalvaRegistros                                                              ;Guarda el estado actual de los registros de proposito general
	MOV Ah, 02h                                                                 ;goto xy
	MOV Bh, 00                                                                  ;Asigna el contenido de 00 a Bh
	MOV dh, gy                                                                  ;posicion del cursor en y
	MOV dl, gx                                                                  ;posicion del cursor en x
	int 10h                                                                     ;Interrupcion 10h (Servicios de Pantalla)
	RecuperaRegistros                                                           ;Reestablece los registros de proposito general al ultimo estado en el que se salvaron
	endm                                                                        ;Fin de la Macro
;______________________MACR_RESTITUIR_____________________  
;Restituye los registros Ax a Dx en ceros 
RestituyeRegistros MACRO                                                    	;Delcaracion de la Macro 'RestituyeRegistros' con los parametros:
	XOR AX,AX                                                                   ;Resituye el registro  AX a ceros por medio de XOR
	XOR BX,BX                                                                   ;Resituye el registro  BX a ceros por medio de XOR
	XOR CX,CX                                                                   ;Resituye el registro  CX a ceros por medio de XOR
	XOR DX,DX                                                                   ;Resituye el registro  DX a ceros por medio de XOR
	endm                                                                        ;Fin de la Macro
;-----------------------------------------------------------------------------------------------------------
;Esta macro llama al procedimiento ImprimirXY que 
;imprime una cadena en las coordendas dadas por X y Y
;el proposito de la macro es enviar los paramteros al procedimiento
;salvando y restaurando posteriormente el estado de los registros
ImprimeXY MACRO cadena,X,Y                                                    	;Delcaracion de la Macro 'Imprime' con los parametros: cadena,X,Y
    
	SalvaRegistros                                                              ;Guarda el estado actual de los registros de proposito general
	RestituyeRegistros                                                          ;Restituye los registros de prosito general en 0
	MOV ch,X                                                                    ;Asigna el contenido de X a ch
	MOV cl,Y                                                                    ;Asigna el contenido de Y a cl
	LEA Bx,cadena                                                               ;asigna a Bx la direccion inicial desplazamiento de la variable 'cadena'

	CALL ImprimirXY                                                             ;Llamada al procedimiento 'ImprimirXY'
	RecuperaRegistros                                                           ;Reestablece los registros de proposito general al ultimo estado en el que se salvaron
	endm                                                                        ;Fin de la Macro
;Esta macro llama al procedimiento Imprimir que 
;imprime una cadena el proposito de la macro es enviar los paramteros al procedimiento
;salvando y restaurando posteriormente el estado de los registros
Imprime MACRO cadena                                                    		;Delcaracion de la Macro 'Imprime' con los parametros: cadena,X,Y
    
	SalvaRegistros                                                              ;Guarda el estado actual de los registros de proposito general
	RestituyeRegistros                                                          ;Restituye los registros de prosito general en 0
	LEA Bx,cadena                                                               ;asigna a Bx la direccion inicial desplazamiento de la variable 'cadena'

	CALL Imprimir																;Llamada al procedimiento 'ImprimirXY'
	RecuperaRegistros                                                           ;Reestablece los registros de proposito general al ultimo estado en el que se salvaron
	endm                                                                        ;Fin de la Macro
;--------[FIN DE LAS MACROS]---------
.model small ;Directiva de inicio simplificada para el modelo de datos 'SMALL'
.stack ;Directiva de inicializacion de la pila simplificada
.data ;Directiva de inicializacion de el segmento de datos simplificado
;-----[INICIA SEGMENTO DE DATOS]-----
registros_tbl      dw 0000h,0000h,0000h,0000h ;guarda el estado previo de los registros
CadenaInicio db ' ','$'
AutoVariable0	db	"aaaaaaaaaaaaaaaaaaaaa"
	db "aaaa",13,"","$"
AutoVariable1	db	"aaaaaaaaaaaaaaaaaaaaa"
	db "aaaa","$"
;----[TERMINA SEGMENTO DE DATOS]-----
.code
;______________________PROC_ImprimirXY (solo debe ser llamado por macro imprimirXY)________________ 
	ImprimirXY proc near                                                        ;Inicia prodecimiento cercano 'ImprimirXY'
	GotoXY ch,cl                                                                ;mueve el cursor a las coordenadas (ch,cl)
	MOV DI,0FFFFh                                                               ;Asigna 0FFFFh a DI
	MOV Ah,02                                                                   ;Asigna 02 a Ah

Impr_siguienteXY:                                                             	;Etiqueta 'Impr_siguiente'
	INC DI                                                                      ;Incrementa en uno el valor de DI
	MOV DL,[Bx+DI]                                                              ;Asigna el contenido de [Bx+DI] a DL
	CMP DL,13d                                                                  ;Compara DL con 13d
	JE NuevaLineaXY                                                               ;Salta si es igual a la etiqueta 'NuevaLinea'
	CMP DL,'$'                                                                  ;Compara DL con '$'
	JE Impr_salirXY                                                               ;Salta si es igual a la etiqueta 'Impr_salir'
	int 21h                                                                     ;Interrupcion 21h (funciones de el DOS API)
	JMP Impr_siguienteXY                                                          ;Salta a la etiqueta 'Impr_siguiente'

NuevaLineaXY:                                                                 	;Etiqueta 'NuevaLinea'
	inc cl                                                                      ;Incrementa en uno el valor de cl
	MOV dh,cl                                                                   ;Asigna el contenido de cl a dh
	GotoXY ch,cl                                                                ;mueve el cursor a las coordenadas (ch,cl)
	JMP Impr_siguienteXY                                                          ;Salta a la etiqueta 'Impr_siguiente'
Impr_salirXY:                                                                 	;Etiqueta 'Impr_salir'
	RET                                                                         ;Retorna el control a la direccion de el SP desde donde se llamo
	ImprimirXY endp                                                             ;Fin del procedimiento
;______________________PROC_Imprimir (solo debe ser llamado por macro imprimir)________________ 
	Imprimir proc near                                                        ;Inicia prodecimiento cercano 'Imprimir'
	MOV DI,0FFFFh                                                               ;Asigna 0FFFFh a DI
	MOV Ah,02                                                                   ;Asigna 02 a Ah

Impr_siguiente:                                                             	;Etiqueta 'Impr_siguiente'
	INC DI                                                                      ;Incrementa en uno el valor de DI
	MOV DL,[Bx+DI]                                                              ;Asigna el contenido de [Bx+DI] a DL
	CMP DL,13d                                                                  ;Compara DL con 13d
	JE NuevaLinea                                                               ;Salta si es igual a la etiqueta 'NuevaLinea'
	CMP DL,'$'                                                                  ;Compara DL con '$'
	JE Impr_salir                                                               ;Salta si es igual a la etiqueta 'Impr_salir'
	int 21h                                                                     ;Interrupcion 21h (funciones de el DOS API)
	JMP Impr_siguiente                                                          ;Salta a la etiqueta 'Impr_siguiente'

NuevaLinea:                                                                 	;Etiqueta 'NuevaLinea'
	inc cl                                                                      ;Incrementa en uno el valor de cl
	MOV dh,cl                                                                   ;Asigna el contenido de cl a dh
	SalvaRegistros 
	GotoXY 0h,dh
	RecuperaRegistros
	JMP Impr_siguiente                                                          ;Salta a la etiqueta 'Impr_siguiente'
Impr_salir:                                                                 	;Etiqueta 'Impr_salir'
	RET                                                                         ;Retorna el control a la direccion de el SP desde donde se llamo
	Imprimir endp                                                             ;Fin del procedimiento
;______________________CLS_PROC______________________
	Cls proc near                                                               ;Inicia prodecimiento cercano 'Cls'
        
	MOV ch, 32                                                                  ;asigna 32 a ch (oculta buffer del cursor)
	MOV Ah, 1                                                                   ;instruccion 1 de la int 10
	int 10h                                                                     ;Interrupcion 10h (Servicios de Pantalla)
    
	MOV Ah, 06h                                                                 ;limpia la pantalla
	MOV al, 00h                                                                 ;coordenas de inicio en x
MOV Bh, 00Fh                                                                	;F:Color Blanco(Fondo),1:Azul(Texto)
	MOV Cx, 0000h                                                               ;coordenas de inicio en y
	MOV Dx, 184Fh                                                               ;tama�o de la pantalla ()
	int 10h                                                                     ;Interrupcion 10h (Servicios de Pantalla)
 
	RET                                                                         ;Retorna el control a la direccion de el SP desde donde se llamo
	Cls endp                                                                    ;Fin del procedimiento
;-----[INICIA PROCEDIMIENTO PRINCIPAL]-----
MiCodigo proc FAR
MOV Ax, @data ;Asigna el contenido de @data a Ax
MOV ds, Ax ;Asigna el contenido de Ax a ds 
Call Cls ;Llamada al procedimiento 'Cls'
ImprimeXY CadenaInicio,0,0
	ImprimeXY	AutoVariable0 0,0
	Imprime	AutoVariable1
PAUSA
MOV Ah,4ch ;Asigna 4ch a Ah
int 21h ;Interrupcion 21h (funciones de el DOS API)
end MiCodigo
;-----[TERMINA PROCEDIMIENTO PRINCIPAL]-----
;----------------[FIN]---------------
