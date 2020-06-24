.model small
.stack
.386
.data
;=========================================================
;VARIABLES TEMPORALES PARA LAS OPERACIONES ARITMETICAS
;PRIMER OPERADOR
SGN1 db 0
NUM1 db 0,0,0,0,0,0,0,0,0
DEC1 db 0,0,0,0,0,0,0,0,0 
;SEGUNDO OPERADOR
SGN2 db 0
NUM2 db 0,0,0,0,0,0,0,0,0
DEC2 db 0,0,0,0,0,0,0,0,0
;VARIABLES DE RESULTADOS GLOBAL
SIGNOT     db 0
ENTEROST   db 0,0,0,0,0,0,0,0,0
DECIMALEST db 0,0,0,0,0,0,0,0,0
;AUXILIARES GLOBALES PARA LAS OPERACIONES
TEMP db 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
AUX  dw 0
POSICION_DIV dw 0
OVERFLOWDIV dw 0
DIVISON dw 0,0,0
;=========================================================
;VARIABLES PARA LA LEECTURA DE CADENAS
CADENA DW 00
LONGUITUD_CADENA DW 0
BLOQUE_ACTUAL DB 0
ULTIMO_SEGMENTO DW 0
ERROR DB 'ERROR','$'
REALLOC DB 10,13,'REALLOC',10,13,'$'
ERROR_ALLOC_CADENA DB 10,13,'OCURRIO UN ERROR RESERVANDO MEMORIA PARA LA CADENA',10,13,'$'
FIN_CADENA DB '$'
;=========================================================
;VARIABLES TEMPORALES PARA LAS OPERACIONES LOGICAS
R_COMPARADOR  db 0
CAD_TEMP1  dw 0
CAD_TEMP2  dw 0
;=========================================================
NUMERO_INVALIDO DB 'NUMERO INVALIDO!','$'
AUXILIAR  DB 0
OVERFLOW DB 'STACK OVERFLOW!','$'
EXCEPCION_NO_CONTROLADA DB 'EXCEPCION NO CONTROLADA:','$'
;VARIABLES DEPENDIENTES DE LA PROGRAMACION

;=======>[NMes-Entero]
SGN_NMes db 0
NUM_NMes db 0,0,0,0,0,0,0,0,0
DEC_NMes db 0,0,0,0,0,0,0,0,0 

;=======>[Mes-Cadena]
Mes DW 00
LONGUITUD_Mes DW 0

;=======>[AUTOVARc763ae084b384843b071bc9d06e9e84a-Cadena]
AUTOVARc763ae084b384843b071bc9d06e9e84a DW 00
LONGUITUD_AUTOVARc763ae084b384843b071bc9d06e9e84a DW 0

;=======>[R_COMPARADOR_1-BitLogico]
R_COMPARADOR_1 DB 0

;=======>[SIGNOT1-Decimal]
SGN_SIGNOT1 db 0
NUM_SIGNOT1 db 0,0,0,0,0,0,0,0,0
DEC_SIGNOT1 db 0,0,0,0,0,0,0,0,0 

;=======>[ENTEROST1-Decimal]
SGN_ENTEROST1 db 0
NUM_ENTEROST1 db 0,0,0,0,0,0,0,0,0
DEC_ENTEROST1 db 0,0,0,0,0,0,0,0,0 

;=======>[DECIMALEST1-Decimal]
SGN_DECIMALEST1 db 0
NUM_DECIMALEST1 db 0,0,0,0,0,0,0,0,0
DEC_DECIMALEST1 db 0,0,0,0,0,0,0,0,0 
.code
;INICIA PROCEDIMIENTO PRINCIPAL
begin proc FAR
MOV AX,@data
MOV DS,AX
;================>[LIMPIAR PANTALLA]<================
mov ax,0600h  ;ah 06(es un recorrido), al 00(pantalla completa)
mov bh,01Fh    ;fondo blanco(7), sobre azul(1)
mov cx,0000h  ;es la esquina superior izquierda reglon: columna
mov dx,184Fh ;es la esquina inferior derecha reglon: columna
int 10h  ;interrupcion que llama al BIOS
;================>[MOVER CURSOR A 0]<================
XOR DX,DX; dl Columna,dh fila
MOV bh,0 ;PAGINA
 mov ah, 02h ;mover cursor
int  10h; ;servicio de video
;================>[CODIGO GENERADO POR EL COMPILADOR]<================
;================[-ALOGAR CADENA-]================
MOV BLOQUE_ACTUAL,12D
CALL ALOGAR_CADENA
MOV SI,CADENA
;=================[ASIGNAR VALOR A LA CADENA]=================
MOV DL,'I'
MOV [SI],DL
INC SI
MOV DL,'n'
MOV [SI],DL
INC SI
MOV DL,'g'
MOV [SI],DL
INC SI
MOV DL,'r'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,'s'
MOV [SI],DL
INC SI
MOV DL,'a'
MOV [SI],DL
INC SI
MOV DL,' '
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,'l'
MOV [SI],DL
INC SI
MOV DL,' '
MOV [SI],DL
INC SI
MOV DL,'n'
MOV [SI],DL
INC SI
MOV DL,0A3H
MOV [SI],DL
INC SI
MOV DL,'m'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,'r'
MOV [SI],DL
INC SI
MOV DL,'o'
MOV [SI],DL
INC SI
MOV DL,' '
MOV [SI],DL
INC SI
MOV DL,'d'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,'l'
MOV [SI],DL
INC SI
MOV DL,' '
MOV [SI],DL
INC SI
MOV DL,'m'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,'s'
MOV [SI],DL
INC SI
MOV DL,':'
MOV [SI],DL
INC SI
MOV DL,FIN_CADENA
MOV [SI],DL
MOV DX,CADENA
MOV AUTOVARc763ae084b384843b071bc9d06e9e84a,DX
MOV DX,AUTOVARc763ae084b384843b071bc9d06e9e84a
CALL IMPRIME_CADENA
LEA DI,SGN_NMes
CALL LEER_ENTERO
;;================[-OPERACION LOGICA-]================
;T1 = NMes=1
;=========[T1 = NMes=1]=========

LEA DX,SGN_NMes
CALL USAR_N1  ;vamos a utilizar T_NMes

;Parte Entera

 MOV SGN2,1H
MOV NUM2[8h],1
MOV NUM2[7h],0
MOV NUM2[6h],0
MOV NUM2[5h],0
MOV NUM2[4h],0
MOV NUM2[3h],0
MOV NUM2[2h],0
MOV NUM2[1h],0
MOV NUM2[0h],0

;Parte Decimal
MOV DEC2[0h],0
MOV DEC2[1h],0
MOV DEC2[2h],0
MOV DEC2[3h],0
MOV DEC2[4h],0
MOV DEC2[5h],0
MOV DEC2[6h],0
MOV DEC2[7h],0
MOV DEC2[8h],0
CALL IGUAL
MOV AL,R_COMPARADOR
MOV R_COMPARADOR_1,AL

MOV AL,1H
CMP R_COMPARADOR,AL
JE IF_VERDADERO_18
JMP INICIO_SINO_18
IF_VERDADERO_18:
;ACCIONES VERDADERAS :)
;================[-ALOGAR CADENA-]================
MOV BLOQUE_ACTUAL,11D
CALL ALOGAR_CADENA
MOV SI,CADENA
;=================[ASIGNAR VALOR A LA CADENA]=================
MOV DL,'E'
MOV [SI],DL
INC SI
MOV DL,'n'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,'r'
MOV [SI],DL
INC SI
MOV DL,'o'
MOV [SI],DL
INC SI
MOV DL,FIN_CADENA
MOV [SI],DL
MOV DX,CADENA
MOV Mes,DX
IF_FALSO_18:
;ACCIONES FALSAS :(
JMP FIN_SINO18
INICIO_SINO_18:
;;================[-OPERACION LOGICA-]================
;T1 = NMes=2
;=========[T1 = NMes=2]=========

LEA DX,SGN_NMes
CALL USAR_N1  ;vamos a utilizar T_NMes

;Parte Entera

 MOV SGN2,1H
MOV NUM2[8h],2
MOV NUM2[7h],0
MOV NUM2[6h],0
MOV NUM2[5h],0
MOV NUM2[4h],0
MOV NUM2[3h],0
MOV NUM2[2h],0
MOV NUM2[1h],0
MOV NUM2[0h],0

;Parte Decimal
MOV DEC2[0h],0
MOV DEC2[1h],0
MOV DEC2[2h],0
MOV DEC2[3h],0
MOV DEC2[4h],0
MOV DEC2[5h],0
MOV DEC2[6h],0
MOV DEC2[7h],0
MOV DEC2[8h],0
CALL IGUAL
MOV AL,R_COMPARADOR
MOV R_COMPARADOR_1,AL

MOV AL,1H
CMP R_COMPARADOR,AL
JE IF_VERDADERO_19
JMP INICIO_SINO_19
IF_VERDADERO_19:
;ACCIONES VERDADERAS :)
;================[-ALOGAR CADENA-]================
MOV BLOQUE_ACTUAL,11D
CALL ALOGAR_CADENA
MOV SI,CADENA
;=================[ASIGNAR VALOR A LA CADENA]=================
MOV DL,'F'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,'b'
MOV [SI],DL
INC SI
MOV DL,'r'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,'r'
MOV [SI],DL
INC SI
MOV DL,'o'
MOV [SI],DL
INC SI
MOV DL,FIN_CADENA
MOV [SI],DL
MOV DX,CADENA
MOV Mes,DX
FIN_SINO18:
JMP FIN_SINO19
INICIO_SINO_19:
;;================[-OPERACION LOGICA-]================
;T1 = NMes=3
;=========[T1 = NMes=3]=========

LEA DX,SGN_NMes
CALL USAR_N1  ;vamos a utilizar T_NMes

;Parte Entera

 MOV SGN2,1H
MOV NUM2[8h],3
MOV NUM2[7h],0
MOV NUM2[6h],0
MOV NUM2[5h],0
MOV NUM2[4h],0
MOV NUM2[3h],0
MOV NUM2[2h],0
MOV NUM2[1h],0
MOV NUM2[0h],0

;Parte Decimal
MOV DEC2[0h],0
MOV DEC2[1h],0
MOV DEC2[2h],0
MOV DEC2[3h],0
MOV DEC2[4h],0
MOV DEC2[5h],0
MOV DEC2[6h],0
MOV DEC2[7h],0
MOV DEC2[8h],0
CALL IGUAL
MOV AL,R_COMPARADOR
MOV R_COMPARADOR_1,AL

MOV AL,1H
CMP R_COMPARADOR,AL
JE IF_VERDADERO_20
JMP INICIO_SINO_20
IF_VERDADERO_20:
;ACCIONES VERDADERAS :)
;================[-ALOGAR CADENA-]================
MOV BLOQUE_ACTUAL,11D
CALL ALOGAR_CADENA
MOV SI,CADENA
;=================[ASIGNAR VALOR A LA CADENA]=================
MOV DL,'M'
MOV [SI],DL
INC SI
MOV DL,'a'
MOV [SI],DL
INC SI
MOV DL,'r'
MOV [SI],DL
INC SI
MOV DL,'z'
MOV [SI],DL
INC SI
MOV DL,'o'
MOV [SI],DL
INC SI
MOV DL,FIN_CADENA
MOV [SI],DL
MOV DX,CADENA
MOV Mes,DX
IF_FALSO_19:
;ACCIONES FALSAS :(
JMP FIN_SINO20
INICIO_SINO_20:
;;================[-OPERACION LOGICA-]================
;T1 = NMes=4
;=========[T1 = NMes=4]=========

LEA DX,SGN_NMes
CALL USAR_N1  ;vamos a utilizar T_NMes

;Parte Entera

 MOV SGN2,1H
MOV NUM2[8h],4
MOV NUM2[7h],0
MOV NUM2[6h],0
MOV NUM2[5h],0
MOV NUM2[4h],0
MOV NUM2[3h],0
MOV NUM2[2h],0
MOV NUM2[1h],0
MOV NUM2[0h],0

;Parte Decimal
MOV DEC2[0h],0
MOV DEC2[1h],0
MOV DEC2[2h],0
MOV DEC2[3h],0
MOV DEC2[4h],0
MOV DEC2[5h],0
MOV DEC2[6h],0
MOV DEC2[7h],0
MOV DEC2[8h],0
CALL IGUAL
MOV AL,R_COMPARADOR
MOV R_COMPARADOR_1,AL

MOV AL,1H
CMP R_COMPARADOR,AL
JE IF_VERDADERO_21
JMP INICIO_SINO_21
IF_VERDADERO_21:
;ACCIONES VERDADERAS :)
;================[-ALOGAR CADENA-]================
MOV BLOQUE_ACTUAL,11D
CALL ALOGAR_CADENA
MOV SI,CADENA
;=================[ASIGNAR VALOR A LA CADENA]=================
MOV DL,'A'
MOV [SI],DL
INC SI
MOV DL,'b'
MOV [SI],DL
INC SI
MOV DL,'r'
MOV [SI],DL
INC SI
MOV DL,'i'
MOV [SI],DL
INC SI
MOV DL,'l'
MOV [SI],DL
INC SI
MOV DL,FIN_CADENA
MOV [SI],DL
MOV DX,CADENA
MOV Mes,DX
FIN_SINO19:
JMP FIN_SINO21
INICIO_SINO_21:
;;================[-OPERACION LOGICA-]================
;T1 = NMes=5
;=========[T1 = NMes=5]=========

LEA DX,SGN_NMes
CALL USAR_N1  ;vamos a utilizar T_NMes

;Parte Entera

 MOV SGN2,1H
MOV NUM2[8h],5
MOV NUM2[7h],0
MOV NUM2[6h],0
MOV NUM2[5h],0
MOV NUM2[4h],0
MOV NUM2[3h],0
MOV NUM2[2h],0
MOV NUM2[1h],0
MOV NUM2[0h],0

;Parte Decimal
MOV DEC2[0h],0
MOV DEC2[1h],0
MOV DEC2[2h],0
MOV DEC2[3h],0
MOV DEC2[4h],0
MOV DEC2[5h],0
MOV DEC2[6h],0
MOV DEC2[7h],0
MOV DEC2[8h],0
CALL IGUAL
MOV AL,R_COMPARADOR
MOV R_COMPARADOR_1,AL

MOV AL,1H
CMP R_COMPARADOR,AL
JE IF_VERDADERO_22
JMP INICIO_SINO_22
IF_VERDADERO_22:
;ACCIONES VERDADERAS :)
;================[-ALOGAR CADENA-]================
MOV BLOQUE_ACTUAL,11D
CALL ALOGAR_CADENA
MOV SI,CADENA
;=================[ASIGNAR VALOR A LA CADENA]=================
MOV DL,'M'
MOV [SI],DL
INC SI
MOV DL,'a'
MOV [SI],DL
INC SI
MOV DL,'y'
MOV [SI],DL
INC SI
MOV DL,'o'
MOV [SI],DL
INC SI
MOV DL,FIN_CADENA
MOV [SI],DL
MOV DX,CADENA
MOV Mes,DX
IF_FALSO_20:
;ACCIONES FALSAS :(
JMP FIN_SINO22
INICIO_SINO_22:
;;================[-OPERACION LOGICA-]================
;T1 = NMes=6
;=========[T1 = NMes=6]=========

LEA DX,SGN_NMes
CALL USAR_N1  ;vamos a utilizar T_NMes

;Parte Entera

 MOV SGN2,1H
MOV NUM2[8h],6
MOV NUM2[7h],0
MOV NUM2[6h],0
MOV NUM2[5h],0
MOV NUM2[4h],0
MOV NUM2[3h],0
MOV NUM2[2h],0
MOV NUM2[1h],0
MOV NUM2[0h],0

;Parte Decimal
MOV DEC2[0h],0
MOV DEC2[1h],0
MOV DEC2[2h],0
MOV DEC2[3h],0
MOV DEC2[4h],0
MOV DEC2[5h],0
MOV DEC2[6h],0
MOV DEC2[7h],0
MOV DEC2[8h],0
CALL IGUAL
MOV AL,R_COMPARADOR
MOV R_COMPARADOR_1,AL

MOV AL,1H
CMP R_COMPARADOR,AL
JE IF_VERDADERO_23
JMP INICIO_SINO_23
IF_VERDADERO_23:
;ACCIONES VERDADERAS :)
;================[-ALOGAR CADENA-]================
MOV BLOQUE_ACTUAL,11D
CALL ALOGAR_CADENA
MOV SI,CADENA
;=================[ASIGNAR VALOR A LA CADENA]=================
MOV DL,'J'
MOV [SI],DL
INC SI
MOV DL,'u'
MOV [SI],DL
INC SI
MOV DL,'n'
MOV [SI],DL
INC SI
MOV DL,'i'
MOV [SI],DL
INC SI
MOV DL,'o'
MOV [SI],DL
INC SI
MOV DL,FIN_CADENA
MOV [SI],DL
MOV DX,CADENA
MOV Mes,DX
FIN_SINO20:
JMP FIN_SINO23
INICIO_SINO_23:
;;================[-OPERACION LOGICA-]================
;T1 = NMes=7
;=========[T1 = NMes=7]=========

LEA DX,SGN_NMes
CALL USAR_N1  ;vamos a utilizar T_NMes

;Parte Entera

 MOV SGN2,1H
MOV NUM2[8h],7
MOV NUM2[7h],0
MOV NUM2[6h],0
MOV NUM2[5h],0
MOV NUM2[4h],0
MOV NUM2[3h],0
MOV NUM2[2h],0
MOV NUM2[1h],0
MOV NUM2[0h],0

;Parte Decimal
MOV DEC2[0h],0
MOV DEC2[1h],0
MOV DEC2[2h],0
MOV DEC2[3h],0
MOV DEC2[4h],0
MOV DEC2[5h],0
MOV DEC2[6h],0
MOV DEC2[7h],0
MOV DEC2[8h],0
CALL IGUAL
MOV AL,R_COMPARADOR
MOV R_COMPARADOR_1,AL

MOV AL,1H
CMP R_COMPARADOR,AL
JE IF_VERDADERO_24
JMP INICIO_SINO_24
IF_VERDADERO_24:
;ACCIONES VERDADERAS :)
;================[-ALOGAR CADENA-]================
MOV BLOQUE_ACTUAL,11D
CALL ALOGAR_CADENA
MOV SI,CADENA
;=================[ASIGNAR VALOR A LA CADENA]=================
MOV DL,'J'
MOV [SI],DL
INC SI
MOV DL,'u'
MOV [SI],DL
INC SI
MOV DL,'l'
MOV [SI],DL
INC SI
MOV DL,'i'
MOV [SI],DL
INC SI
MOV DL,'o'
MOV [SI],DL
INC SI
MOV DL,FIN_CADENA
MOV [SI],DL
MOV DX,CADENA
MOV Mes,DX
IF_FALSO_21:
;ACCIONES FALSAS :(
JMP FIN_SINO24
INICIO_SINO_24:
;;================[-OPERACION LOGICA-]================
;T1 = NMes=8
;=========[T1 = NMes=8]=========

LEA DX,SGN_NMes
CALL USAR_N1  ;vamos a utilizar T_NMes

;Parte Entera

 MOV SGN2,1H
MOV NUM2[8h],8
MOV NUM2[7h],0
MOV NUM2[6h],0
MOV NUM2[5h],0
MOV NUM2[4h],0
MOV NUM2[3h],0
MOV NUM2[2h],0
MOV NUM2[1h],0
MOV NUM2[0h],0

;Parte Decimal
MOV DEC2[0h],0
MOV DEC2[1h],0
MOV DEC2[2h],0
MOV DEC2[3h],0
MOV DEC2[4h],0
MOV DEC2[5h],0
MOV DEC2[6h],0
MOV DEC2[7h],0
MOV DEC2[8h],0
CALL IGUAL
MOV AL,R_COMPARADOR
MOV R_COMPARADOR_1,AL

MOV AL,1H
CMP R_COMPARADOR,AL
JE IF_VERDADERO_25
JMP INICIO_SINO_25
IF_VERDADERO_25:
;ACCIONES VERDADERAS :)
;================[-ALOGAR CADENA-]================
MOV BLOQUE_ACTUAL,11D
CALL ALOGAR_CADENA
MOV SI,CADENA
;=================[ASIGNAR VALOR A LA CADENA]=================
MOV DL,'A'
MOV [SI],DL
INC SI
MOV DL,'g'
MOV [SI],DL
INC SI
MOV DL,'o'
MOV [SI],DL
INC SI
MOV DL,'s'
MOV [SI],DL
INC SI
MOV DL,'t'
MOV [SI],DL
INC SI
MOV DL,'o'
MOV [SI],DL
INC SI
MOV DL,FIN_CADENA
MOV [SI],DL
MOV DX,CADENA
MOV Mes,DX
FIN_SINO21:
JMP FIN_SINO25
INICIO_SINO_25:
;;================[-OPERACION LOGICA-]================
;T1 = NMes=9
;=========[T1 = NMes=9]=========

LEA DX,SGN_NMes
CALL USAR_N1  ;vamos a utilizar T_NMes

;Parte Entera

 MOV SGN2,1H
MOV NUM2[8h],9
MOV NUM2[7h],0
MOV NUM2[6h],0
MOV NUM2[5h],0
MOV NUM2[4h],0
MOV NUM2[3h],0
MOV NUM2[2h],0
MOV NUM2[1h],0
MOV NUM2[0h],0

;Parte Decimal
MOV DEC2[0h],0
MOV DEC2[1h],0
MOV DEC2[2h],0
MOV DEC2[3h],0
MOV DEC2[4h],0
MOV DEC2[5h],0
MOV DEC2[6h],0
MOV DEC2[7h],0
MOV DEC2[8h],0
CALL IGUAL
MOV AL,R_COMPARADOR
MOV R_COMPARADOR_1,AL

MOV AL,1H
CMP R_COMPARADOR,AL
JE IF_VERDADERO_26
JMP INICIO_SINO_26
IF_VERDADERO_26:
;ACCIONES VERDADERAS :)
;================[-ALOGAR CADENA-]================
MOV BLOQUE_ACTUAL,11D
CALL ALOGAR_CADENA
MOV SI,CADENA
;=================[ASIGNAR VALOR A LA CADENA]=================
MOV DL,'S'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,'p'
MOV [SI],DL
INC SI
MOV DL,'t'
MOV [SI],DL
INC SI
MOV DL,'i'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,'m'
MOV [SI],DL
INC SI
MOV DL,'b'
MOV [SI],DL
INC SI
MOV DL,'r'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,FIN_CADENA
MOV [SI],DL
MOV DX,CADENA
MOV Mes,DX
IF_FALSO_22:
;ACCIONES FALSAS :(
JMP FIN_SINO26
INICIO_SINO_26:
;;================[-OPERACION LOGICA-]================
;T1 = NMes=10
;=========[T1 = NMes=10]=========

LEA DX,SGN_NMes
CALL USAR_N1  ;vamos a utilizar T_NMes

;Parte Entera

 MOV SGN2,1H
MOV NUM2[8h],0
MOV NUM2[7h],1
MOV NUM2[6h],0
MOV NUM2[5h],0
MOV NUM2[4h],0
MOV NUM2[3h],0
MOV NUM2[2h],0
MOV NUM2[1h],0
MOV NUM2[0h],0

;Parte Decimal
MOV DEC2[0h],0
MOV DEC2[1h],0
MOV DEC2[2h],0
MOV DEC2[3h],0
MOV DEC2[4h],0
MOV DEC2[5h],0
MOV DEC2[6h],0
MOV DEC2[7h],0
MOV DEC2[8h],0
CALL IGUAL
MOV AL,R_COMPARADOR
MOV R_COMPARADOR_1,AL

MOV AL,1H
CMP R_COMPARADOR,AL
JE IF_VERDADERO_27
JMP INICIO_SINO_27
IF_VERDADERO_27:
;ACCIONES VERDADERAS :)
;================[-ALOGAR CADENA-]================
MOV BLOQUE_ACTUAL,11D
CALL ALOGAR_CADENA
MOV SI,CADENA
;=================[ASIGNAR VALOR A LA CADENA]=================
MOV DL,'O'
MOV [SI],DL
INC SI
MOV DL,'c'
MOV [SI],DL
INC SI
MOV DL,'t'
MOV [SI],DL
INC SI
MOV DL,'u'
MOV [SI],DL
INC SI
MOV DL,'b'
MOV [SI],DL
INC SI
MOV DL,'r'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,FIN_CADENA
MOV [SI],DL
MOV DX,CADENA
MOV Mes,DX
FIN_SINO22:
JMP FIN_SINO27
INICIO_SINO_27:
;;================[-OPERACION LOGICA-]================
;T1 = NMes=11
;=========[T1 = NMes=11]=========

LEA DX,SGN_NMes
CALL USAR_N1  ;vamos a utilizar T_NMes

;Parte Entera

 MOV SGN2,1H
MOV NUM2[8h],1
MOV NUM2[7h],1
MOV NUM2[6h],0
MOV NUM2[5h],0
MOV NUM2[4h],0
MOV NUM2[3h],0
MOV NUM2[2h],0
MOV NUM2[1h],0
MOV NUM2[0h],0

;Parte Decimal
MOV DEC2[0h],0
MOV DEC2[1h],0
MOV DEC2[2h],0
MOV DEC2[3h],0
MOV DEC2[4h],0
MOV DEC2[5h],0
MOV DEC2[6h],0
MOV DEC2[7h],0
MOV DEC2[8h],0
CALL IGUAL
MOV AL,R_COMPARADOR
MOV R_COMPARADOR_1,AL

MOV AL,1H
CMP R_COMPARADOR,AL
JE IF_VERDADERO_28
JMP INICIO_SINO_28
IF_VERDADERO_28:
;ACCIONES VERDADERAS :)
;================[-ALOGAR CADENA-]================
MOV BLOQUE_ACTUAL,11D
CALL ALOGAR_CADENA
MOV SI,CADENA
;=================[ASIGNAR VALOR A LA CADENA]=================
MOV DL,'N'
MOV [SI],DL
INC SI
MOV DL,'o'
MOV [SI],DL
INC SI
MOV DL,'v'
MOV [SI],DL
INC SI
MOV DL,'i'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,'m'
MOV [SI],DL
INC SI
MOV DL,'b'
MOV [SI],DL
INC SI
MOV DL,'r'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,FIN_CADENA
MOV [SI],DL
MOV DX,CADENA
MOV Mes,DX
IF_FALSO_23:
;ACCIONES FALSAS :(
JMP FIN_SINO28
INICIO_SINO_28:
;;================[-OPERACION LOGICA-]================
;T1 = NMes=12
;=========[T1 = NMes=12]=========

LEA DX,SGN_NMes
CALL USAR_N1  ;vamos a utilizar T_NMes

;Parte Entera

 MOV SGN2,1H
MOV NUM2[8h],2
MOV NUM2[7h],1
MOV NUM2[6h],0
MOV NUM2[5h],0
MOV NUM2[4h],0
MOV NUM2[3h],0
MOV NUM2[2h],0
MOV NUM2[1h],0
MOV NUM2[0h],0

;Parte Decimal
MOV DEC2[0h],0
MOV DEC2[1h],0
MOV DEC2[2h],0
MOV DEC2[3h],0
MOV DEC2[4h],0
MOV DEC2[5h],0
MOV DEC2[6h],0
MOV DEC2[7h],0
MOV DEC2[8h],0
CALL IGUAL
MOV AL,R_COMPARADOR
MOV R_COMPARADOR_1,AL

MOV AL,1H
CMP R_COMPARADOR,AL
JE IF_VERDADERO_29
JMP INICIO_SINO_29
IF_VERDADERO_29:
;ACCIONES VERDADERAS :)
;================[-ALOGAR CADENA-]================
MOV BLOQUE_ACTUAL,11D
CALL ALOGAR_CADENA
MOV SI,CADENA
;=================[ASIGNAR VALOR A LA CADENA]=================
MOV DL,'D'
MOV [SI],DL
INC SI
MOV DL,'i'
MOV [SI],DL
INC SI
MOV DL,'c'
MOV [SI],DL
INC SI
MOV DL,'i'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,'m'
MOV [SI],DL
INC SI
MOV DL,'b'
MOV [SI],DL
INC SI
MOV DL,'r'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,FIN_CADENA
MOV [SI],DL
MOV DX,CADENA
MOV Mes,DX
FIN_SINO23:
JMP FIN_SINO29
INICIO_SINO_29:
;================[-ALOGAR CADENA-]================
MOV BLOQUE_ACTUAL,11D
CALL ALOGAR_CADENA
MOV SI,CADENA
;=================[ASIGNAR VALOR A LA CADENA]=================
MOV DL,'I'
MOV [SI],DL
INC SI
MOV DL,'n'
MOV [SI],DL
INC SI
MOV DL,'v'
MOV [SI],DL
INC SI
MOV DL,'a'
MOV [SI],DL
INC SI
MOV DL,'l'
MOV [SI],DL
INC SI
MOV DL,'i'
MOV [SI],DL
INC SI
MOV DL,'d'
MOV [SI],DL
INC SI
MOV DL,'o'
MOV [SI],DL
INC SI
MOV DL,FIN_CADENA
MOV [SI],DL
MOV DX,CADENA
MOV Mes,DX
IF_FALSO_24:
;ACCIONES FALSAS :(
FIN_SINO24:
IF_FALSO_25:
;ACCIONES FALSAS :(
FIN_SINO25:
IF_FALSO_26:
;ACCIONES FALSAS :(
FIN_SINO26:
IF_FALSO_27:
;ACCIONES FALSAS :(
FIN_SINO27:
IF_FALSO_28:
;ACCIONES FALSAS :(
FIN_SINO28:
IF_FALSO_29:
;ACCIONES FALSAS :(
FIN_SINO29:
MOV DX,Mes
CALL IMPRIME_CADENA
;==============>[FIN CODIGO GENERADO POR EL COMPILADOR]<==============
XOR AX,AX
INT 16H
;================>[LIBERAR TODA LA MEMORIA DE CADENAS]<================
MOV BX,Mes
CALL LIBERAR_CADENA
MOV BX,AUTOVARc763ae084b384843b071bc9d06e9e84a
CALL LIBERAR_CADENA
MOV AH,4CH
INT 21H
RET

;==============>[PROCEDIMIENTOS INTERNOS GENERADOS POR EL COMPILADOR]<==============
;====================================[USAR_N1]====================================    
    USAR_N1 PROC NEAR 
    MOV SI,018D ;DESPLAZAMIENTO POR EL CUAL SE ALCANZA LA ULTIMA CIFRA CONTANDO A CERO HASTA VARIABLE SIGNO
    ADD SI,DX 
    MOV DI,018D ;POSICION DEL ULTIMO NUMERO DEL DESTINO
    
    cpy_n1:
    MOV AL,[SI] 
    MOV SGN1[DI],AL
    
    DEC SI
    DEC DI
    JNS cpy_n1   
    
    RET    
    USAR_N1 ENDP 
        
;====================================[USAR_N2]====================================
    USAR_N2 PROC NEAR
    MOV SI,018D ;DESPLAZAMIENTO POR EL CUAL SE ALCANZA LA ULTIMA CIFRA CONTANDO A CERO HASTA VARIABLE SIGNO
    ADD SI,DX 
    MOV DI,018D ;POSICION DEL ULTIMO NUMERO DEL DESTINO
    
    cpy_n2:
    MOV AL,[SI] 
    MOV SGN2[DI],AL
    
    DEC SI
    DEC DI
    JNS cpy_n2   
    
    RET       
    USAR_N2 ENDP 
;====================================[SALVAR_TEMPORAL]====================================    
    SALVAR_TEMPORAL PROC NEAR
    MOV SI,018D ;Posicion del ultimo digito decimal
    MOV DI,018D
    ADD DI,DX 
        
    cpy_temp:
    MOV AL,SIGNOT[SI] 
    MOV [DI],AL
    
    DEC DI
    DEC SI
    JNS cpy_temp   
    
    RET       
    SALVAR_TEMPORAL ENDP
    
;====================================[LIMPIAR]====================================    
    LIMPIAR PROC NEAR  
        MOV SI,018D ;Posicion del ultimo digito decimal
        MOV CX,DX
        
        limpiar_siguienteT: 
        MOV DI,SI
        ADD DI,CX 
        MOV AL,0H
        MOV [DI],AL
        ;MOV SIGNOT[SI],0H  
        
        DEC SI
        JNS limpiar_siguienteT 
        RET
    LIMPIAR ENDP 
;==============>[ALOGAR_CADENA]<==============
        ALOGAR_CADENA PROC NEAR 
    
        ;MOV BLOQUE_ACTUAL,0AH ;INDICA EL BLOQUE EN EL QUE SE ESTA ESCRIBIENDO
    
        ;RESERVAR LA MEOMRIA PARA LEER LA CADENA (MALLOC)
        ;FUNCION AH,48H       
    
        ;PARAMETROS DE ENTRADA:
        ;BX - TAMANIO DE EL BLOQUE (EN PARRAFOS GRUPOS DE 16 BYTES)    
    
        ;PARAMETOS DE SALIDA:  
        ;SI NO HAY ERROR BANDERA DE ACARREO LIMPIA
        ;AX - APUNTA A LA DIRECCION DE INICIO DEL NUEVO BLOQUE RESERVADO
    
        ;SI HAY ERROR ACARREO CON VALOR,
        ;BX - TAMANIO MAXIMO DE MEMORIA DISPONIBLE
        ;AX CODIGO DE ERROR (7 O 8)
              
        MOV AH,48H
        XOR BX,BX
        MOV BL,BLOQUE_ACTUAL ;16*BLOQUE_ACTUAL BYTES DE INICIO
        INT 21H 
        MOV CADENA,AX 
        JC ERROR_ALLOC
        JMP FIN_ALOGAR_CADENA
 
ERROR_ALLOC:     
        MOV CADENA,DS
        XOR AX,AX
        MOV AL,BLOQUE_ACTUAL
        MOV AH,0FH
        MUL AH 
          
        MOV DI,CADENA
        ADD DI,AX
        MOV CX,ULTIMO_SEGMENTO
        ADD DI,CX
        MOV CADENA,DI
    
        ;MOV AH,09H
        ;LEA DX,ERROR_ALLOC_CADENA
        ;INT 21H    
    
        FIN_ALOGAR_CADENA:
        MOV CX,CADENA
        MOV ULTIMO_SEGMENTO,CX
        RET
         
    ALOGAR_CADENA ENDP

        ;==============>[ALOGAR_CADENA]<==============
        LIBERAR_CADENA PROC NEAR ;(FREE)
            ;Funcion AH   49h  
            ;PARAMETROS DE ENTRADA: ES:0-DIRECCION BASE CERO DEL BLOQUE PARA LIBERAR
            ;PARAMETROS DE SALIDA: SI EL ACARREO ESTA ACTIVO AX CONTIENE EL CODIGO DE 
            ;ERROR (7,9)
      
            MOV AH,49H 
            MOV ES,BX
            INT 21H
   
            RET
            LIBERAR_CADENA ENDP 
LEER_CADENA PROC NEAR 
        XOR CX,CX
      
 
     
        MOV CADENA,DS
        XOR AX,AX
        MOV AL,BLOQUE_ACTUAL
        MOV AH,0FH
        MUL AH 
          
        MOV DI,CADENA
        ADD DI,AX
        MOV CX,ULTIMO_SEGMENTO
        ADD DI,CX
        MOV CADENA,DI
        XOR CX,CX 
        ;MOV AH,09H
        ;LEA DX,ERROR_ALLOC_CADENA
        ;INT 21H    
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;LECTURA
AGREGAR_CARACTER:  
    MOV SI,CADENA
    MOV AH,08H
    INT 21H
    
    CMP AL,08H
    JE RETROCEDER
    CMP AL,0DH ;TECLA ENTER
    JE LEER_CADENA_SALIR
    
    MOV DI,SI
    ADD DI,CX 
    MOV [DI],AL 
    INC CX
    
    ;IMPRIMIR EL CARACTER EN PANTALLA
    MOV DL,AL
    MOV AH,02H
    INT 21H
    ;;REVISAR SI YA NOS ACABAMOS EL BLOQUE
    
    MOV dx,0 ; 
    MOV AX,CX ; 
    MOV BX,0FH
    DIV BX ;
    
    CMP AL,BLOQUE_ACTUAL 
    JA REALLOC_CADENA
    JMP AGREGAR_CARACTER
    ;;;;;;;;;;;;;;;;;;;;; 
            REALLOC_CADENA:
        ;MOV AH,09H
        ;LEA DX,REALLOC
        ;INT 21H
        ;;AUMENTAR EL BLOQUE Y RESERVAR UN PARRAFO MAS (REALLOC) 
        ;FUNCION AH,04AH    
        
        ;PARAMETROS ENTRADA
        ;ES - DIRECCION DEL BLOQUE A MODIFICAR
        ;BX NUEVO TAMANIO DE BLOQUE  
        
        ;PARAMETROS DE SALIDA
        ;SI HAY ERROR ACARREO CON VALOR,
        ;BX - TAMANIO MAXIMO DE MEMORIA DISPONIBLE
        ;AX CODIGO DE ERROR (7,8,9)        
            
            INC BLOQUE_ACTUAL
            
            MOV AH,04AH
            MOV ES,DI
            MOV BL,BLOQUE_ACTUAL
            INT 21H
            ;JC ERROR_REALLOC
            JMP AGREGAR_CARACTER
            ERROR_REALLOC:
            LEA DX,ERROR
            MOV AH,09H
            INT 21H 
    ;;;;;;;;;;;;;;;;;;;;;;     
    ;;;;;;;;;;;;;
    RETROCEDER:
    MOV AH,02H
    MOV DL,08H ;backspace 
    INT 21H 
    ;
    MOV AH,02H  
    MOV DL,00H ;caracter nulo para borra el caracter
    INT 21H
    ;
    MOV AH,02H  
    MOV DL,08H;backspace 
    INT 21H
    
    CMP CX,00H
    JE no_decrementar_posicion
    DEC CX                  
    
    no_decrementar_posicion:

    JMP AGREGAR_CARACTER
    ;;;;;;;;;;;;;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;    
        LEER_CADENA_SALIR:
        
        MOV AL,FIN_CADENA
        MOV DI,SI
        ADD DI,CX         
        MOV [DI],AL 
        INC CX
    
        MOV CX,CADENA
        MOV ULTIMO_SEGMENTO,CX
   
   RET      
   LEER_CADENA ENDP
IGUAL PROC NEAR

    MOV AL,SGN1
    MOV AH,SGN2
    CMP AL,AH
    JE signo_igual_igualdad 
    
    MOV R_COMPARADOR,0H
    JMP salir_igualdad               
    
signo_igual_igualdad:    
    MOV SI,0FFFFH
    ;  
    siguiente_igualdad:
        INC SI 
        CMP SI,0012 ;Posicion del ultimo digito decimal ~
        JA iguales
        
        MOV AL,NUM1[SI]
        MOV AH,NUM2[SI]
        CMP AL,AH
        JE siguiente_igualdad
        JNE diferente
    ;     
    
diferente:
    MOV R_COMPARADOR,0H
    JMP salir_igualdad
iguales:
    MOV R_COMPARADOR,1H
    
salir_igualdad:    
    RET    
IGUAL ENDP


MAYOR_QUE PROC NEAR

    MOV AL,SGN1
    MOV AH,SGN2
    CMP AL,AH
    JE signo_igual_mayorque
    JL menor_que_exit
    JC mayor_que_exit
    
    MOV R_COMPARADOR,0H
    JMP salir_mayorque               
    
signo_igual_mayorque:    
    MOV SI,0FFFFH
    ;  
    siguiente_mayorque:
        INC SI 
        CMP SI,018D ;Posicion del ultimo digito decimal ~
        JA iguales
        
        MOV AL,NUM1[SI]
        MOV AH,NUM2[SI]
        CMP AL,AH
        JA  mayor_que_exit    
        JL  menor_que_exit
        JE  siguiente_mayorque
        JMP siguiente_mayorque
    ;     
    
menor_que_exit:
    MOV R_COMPARADOR,0H
    JMP salir_igualdad
mayor_que_exit:
    MOV R_COMPARADOR,1H
    
salir_mayorque:    
    RET    
MAYOR_QUE ENDP

MENOR_QUE PROC NEAR

CALL MAYOR_QUE
MOV AL,R_COMPARADOR
CMP AL,01H
JE A_CERO 
JMP A_UNO

A_CERO:
MOV R_COMPARADOR,0H
JMP SALIR_MENOR_QUE

A_UNO:
MOV R_COMPARADOR,1H

SALIR_MENOR_QUE:
RET   
MENOR_QUE ENDP

MENOR_QUE_IGUAL PROC NEAR

CALL MENOR_QUE  
MOV AL,R_COMPARADOR
CMP AL,01H
JE VERDADERO
CALL IGUAL
MOV AL,R_COMPARADOR
CMP AL,01H
JE VERDADERO
JMP FALSO
 
VERDADERO:
MOV R_COMPARADOR,01H
JMP FIN_COMPARACION
FALSO:
MOV R_COMPARADOR,0H

FIN_COMPARACION: 
RET
MENOR_QUE_IGUAL ENDP

IGUAL_CADENA PROC NEAR
    MOV R_COMPARADOR,1H
    
    MOV CX,0FFFFH
    XOR AX,AX
siguiente_iguales_cadena: 
    CMP AL,FIN_CADENA
    JE fin_iguales_cadena 
       
    INC CX
    
    MOV DI,CAD_TEMP1
    
    MOV SI,CAD_TEMP2
    
    ADD DI,CX
    ADD SI,CX
    
    MOV AL,[DI]
    MOV AH,[SI]
    
    CMP AL,AH
    JE siguiente_iguales_cadena
    JNE diferentes_iguales_cadena
    


    
diferentes_iguales_cadena:
MOV R_COMPARADOR,0H    

fin_iguales_cadena:
RET    
IGUAL_CADENA ENDP
DIFERENTE_CADENA PROC NEAR
CALL IGUAL_CADENA

MOV AL,R_COMPARADOR
CMP AL,01H
JE A_CERO_C 
JMP A_UNO_C

A_CERO_C:
MOV R_COMPARADOR,0H
JMP SALIR_DIFERENTE_C

A_UNO_C:
MOV R_COMPARADOR,1H

SALIR_DIFERENTE_C:
RET   
DIFERENTE_CADENA ENDP

ES_NUMERO PROC NEAR
    
    CMP AL,30H
    JL  numero_invalido_enteros3
    CMP AL,39H
    JA numero_invalido_enteros3 
    
    SUB AL,30H 
    RET
    numero_invalido_enteros3:
    MOV AL,'$'
    
RET    
ENDP
LEER_ENTERO PROC NEAR 

    ;LIMPIAR VARIABLES
    MOV DX,DI
    CALL LIMPIAR

    MOV SI,DI
    XOR CX,CX
    
    MOV DL,01H
    MOV [DI],DL

    MOV AH,01H
    INT 21H
    
    ;REVISAR SIGNO
    MOV AH,'-'
    CMP AH,AL
    JE lectura_entero_negativo2
    MOV AH,'+'
    CMP AH,AL
    JE lectura_entero_positivo2
    JMP validar_numero
    
    lectura_entero_negativo2:
    MOV DL,0FFH
    MOV [DI],DL
    JMP siguiente_entero
    
    lectura_entero_positivo2:
    MOV DL,01H
    MOV [DI],DL
    JMP siguiente_entero
    
    siguiente_entero:
    MOV AH,01H
    INT 21H    
    ;;;;;;;;;
        validar_numero: 
        CMP CX,09H
        JAE overflow_numerico2
            CMP AL,08H
            JE retroceso_entero 
            CMP AL,0DH
            JE fin_ingreso_entero
        CALL ES_NUMERO
        MOV AH,'$'
        CMP AL,AH
        JNE salvar_numero
        JMP numero_invalido_enteros2   
    ;;;;;;;;; 
    salvar_numero:
    INC DI
    INC CX
    MOV [DI],AL
    JMP siguiente_entero
    
    retroceso_entero:
    MOV DL,00H
    MOV AH,02H
    INT 21H
    MOV DL,8H
    INT 21H    
        
    CMP CX,0H
    JE siguiente_entero
    DEC CX
    MOV DL,0H
    MOV [DI],DL
    DEC DI
    JMP siguiente_entero 
    
fin_ingreso_entero:
    ;AJUSTE
    CMP CX,00H
    JE entero_ingreso_vacio
    CMP CX,09H
    JL ajustar_entero
    
    ajustar_entero: 
    MOV AX,SI ;SALVAR DIRECCION
    
    
    MOV DI,SI
    ADD DI,CX ;FUENTE 
    
    ADD SI,09H ;DESTINO
    
    
    ajusta_siguiente_digito_entero:
        
        MOV AL,[DI]
        MOV [SI],AL
        MOV DL,0H
        MOV [DI],DL
        
        DEC SI
        DEC DI   
    
    DEC CX    
    CMP CX,00H
    JE salir_ajuste_entero
    JMP ajusta_siguiente_digito_entero
entero_ingreso_vacio:
salir_ajuste_entero:    
RET       
overflow_numerico2: 
LEA DI,OVERFLOW  
CALL EXCEPCION  
numero_invalido_enteros2:
LEA DI,NUMERO_INVALIDO
CALL EXCEPCION               
RET               
LEER_ENTERO ENDP
EXCEPCION PROC NEAR

MOV AH, 06h    ; Scroll up function
XOR AL, AL     ; Clear entire screen
XOR CX, CX     ; Upper left corner CH=row, CL=column
MOV DX, 184FH  ; lower right corner DH=row, DL=column 
MOV BH, 4Eh    ; YellowOnBlue
INT 10H

MOV DL,1AH
MOV DH,0AH
MOV bh,0 ;PAGINA
mov ah, 02h ;mover cursor
int  10h; ;servicio de video 

 
LEA DX,EXCEPCION_NO_CONTROLADA
MOV AH,09H
INT 21H
 
MOV DL,1AH
MOV DH,0CH
MOV bh,0 ;PAGINA
mov ah, 02h ;mover cursor
int  10h; ;servicio de video 
 
MOV DX,DI
MOV AH,09H
INT 21H         

XOR AX,AX
INT 16H

MOV AH,4CH
INT 21H
              
EXCEPCION ENDP
IMPRIME_CADENA PROC NEAR
    
    MOV DI,DX
    
    siguiente_caracter_impresion2:
    MOV DL,[DI]
    CMP DL,015H ;ALIAS IMPRIMIBLE PARA $
    JE imprime_signo_pesos
    CMP DL,'$'
    JE salir_IMPRIME_CADENA
    
    MOV AH,02H
    INT 21H
    
    siguiente_caracter_impresion:
    INC DI
    JMP siguiente_caracter_impresion2
    
    
    imprime_signo_pesos:
    MOV DL,'$'
    MOV AH,02H
    INT 21H
    
    
    JMP siguiente_caracter_impresion
    
salir_IMPRIME_CADENA:
RET    
IMPRIME_CADENA ENDP
begin endp
end begin
