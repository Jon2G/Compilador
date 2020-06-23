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
OVERFLOW DB 'STACK OVERFLOW!','$'
EXCEPCION_NO_CONTROLADA DB 'EXCEPCION NO CONTROLADA:','$'
;VARIABLES DEPENDIENTES DE LA PROGRAMACION

;=======>[Y2-Entero]
SGN_Y2 db 0
NUM_Y2 db 0,0,0,0,0,0,0,0,0
DEC_Y2 db 0,0,0,0,0,0,0,0,0 

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

;=======>[AUTOVAR2c1b6135ed6449349cd2487326261dea-Cadena]
AUTOVAR2c1b6135ed6449349cd2487326261dea DW 00
LONGUITUD_AUTOVAR2c1b6135ed6449349cd2487326261dea DW 0
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
INICIO_FOR_17:
;================[-CODIGO TRES DIRECCIONES-]================

;Parte Entera

 MOV SGN_Y2,1H
MOV NUM_Y2[8h],0
MOV NUM_Y2[7h],0
MOV NUM_Y2[6h],0
MOV NUM_Y2[5h],0
MOV NUM_Y2[4h],0
MOV NUM_Y2[3h],0
MOV NUM_Y2[2h],0
MOV NUM_Y2[1h],0
MOV NUM_Y2[0h],0

;Parte Decimal
MOV DEC_Y2[0h],0
MOV DEC_Y2[1h],0
MOV DEC_Y2[2h],0
MOV DEC_Y2[3h],0
MOV DEC_Y2[4h],0
MOV DEC_Y2[5h],0
MOV DEC_Y2[6h],0
MOV DEC_Y2[7h],0
MOV DEC_Y2[8h],0
INICIO_CICLO_17:
LEA DI,SGN_Y2
CALL MOSTRAR_RESULTADO
;================[-ALOGAR CADENA-]================
MOV BLOQUE_ACTUAL,11D
CALL ALOGAR_CADENA
MOV SI,CADENA
;=================[ASIGNAR VALOR A LA CADENA]=================
MOV DL,10
MOV [SI],DL
INC SI
MOV DL,13
MOV [SI],DL
INC SI
MOV DL,FIN_CADENA
MOV [SI],DL
MOV DX,CADENA
MOV AUTOVAR2c1b6135ed6449349cd2487326261dea,DX
MOV DX,AUTOVAR2c1b6135ed6449349cd2487326261dea
CALL IMPRIME_CADENA
;================[-CODIGO TRES DIRECCIONES-]================
;=========[T1 = Y2+1]=========

LEA DX,SGN_Y2
CALL USAR_N1  ;vamos a utilizar T_Y2

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
CALL SUMA
LEA DX,SGN_SIGNOT1
CALL SALVAR_TEMPORAL

LEA DX,SGN_Y2
CALL SALVAR_TEMPORAL
;================[-OPERACION LOGICA-]================
;T1 = Y2<=10
;=========[T1 = Y2<=10]=========

LEA DX,SGN_Y2
CALL USAR_N1  ;vamos a utilizar T_Y2

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
CALL MENOR_QUE_IGUAL
MOV AL,R_COMPARADOR
MOV R_COMPARADOR_1,AL

MOV AL,1H
CMP R_COMPARADOR,AL
JE INICIO_CICLO_17
;==============>[FIN CODIGO GENERADO POR EL COMPILADOR]<==============
XOR AX,AX
INT 16H
;================>[LIBERAR TODA LA MEMORIA DE CADENAS]<================
MOV BX,AUTOVAR2c1b6135ed6449349cd2487326261dea
CALL LIBERAR_CADENA
MOV AH,4CH
INT 21H
RET

;==============>[PROCEDIMIENTOS INTERNOS GENERADOS POR EL COMPILADOR]<==============
;====================================[SUMA]====================================        
    SUMA PROC NEAR
    ;LIMPIAR VARIABLES
    LEA DX,SIGNOT
    CALL LIMPIAR
    ;        
    ;COMPROBAR SI SE DEBERIA USAR LA RESTA EN SU LUGAR (-)(-) 
    MOV AL,SGN1
    CMP AL,SGN2
    JE  signos_iguales_suma
    JMP signos_diferentes_suma 
    
    signos_iguales_suma:
;;;;
MOV SIGNOT,AL
;;;
    CMP SGN1,0FFH
    JE sumar_y_negar  
    JNE sumar_con_normalidad
    
    signos_diferentes_suma:
    CMP SGN2,0FFH
    ;si el segundo es negativo pero el primero no,podemos restar tal cual :)
    JNE revisar_signo1_suma
    CALL RESTA
    RET
    revisar_signo1_suma:
    ;si el primero es negativo pero el segundo no,debemos invertir los operandos
    ;y restar normalmente...
        MOV SI,08H

            invierte_siguiente_suma:
            ;
            MOV AL,SGN1[SI]
            MOV AH,SGN2[SI] 
            
            MOV SGN2[SI],AL  
            MOV SGN1[SI],AH
            ;     
            DEC SI
            JNS invierte_siguiente_suma
        
        CALL RESTA
        
    RET
    sumar_y_negar:
    MOV SIGNOT,0FFH
    sumar_con_normalidad:   
            ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
            MOV SI,017D ;NUMERO MAS A LA IZQUIERDA             
        
                siguiente_sumando:
            

                    MOV AL,NUM2[SI] ;OP_1
                    ADD AL,NUM1[SI]
                    AAM
                    ;;===
                        ;

                        ADD AL,ENTEROST[SI]           
                        MOV ENTEROST[SI],AL

                   
                        ADD AH,ENTEROST[SI-1]
                        MOV ENTEROST[SI-1],AH  
                    
                        ;
                    ;;===
                    DEC SI
                    JNS siguiente_sumando                      
                    

            ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;       
            
            ;Ajustar acarreos        
        
            MOV SI,018D ;Posicion del ultimo digito decimal ~  
        
            siguiente_acarreo:
        
            MOV AL,ENTEROST[SI]
            CMP AL,0AH
            JL sin_acarreo 
            AAM
            MOV ENTEROST[SI],AL
            ADD ENTEROST[SI-1H],AH

            sin_acarreo:  
            DEC SI
            JNS siguiente_acarreo
            RET
        
    SUMA ENDP
;====================================[RESTA]====================================        
    RESTA PROC NEAR  
    ;LIMPIAR OPERADORES 
    LEA DX,SIGNOT
    CALL LIMPIAR               
    ;;ACOMODAR LOS OPERANDOS DE MODO QUE EL MAYOR QUEDE EN NUM1
    MOV SI,0H
    acomodar_siguiente:
        MOV AL,NUM1[SI]
        MOV AH,NUM2[SI]
        CMP AL,AH
        JL invertir_operandos
        JA resta_es_positivo  
        INC SI
        CMP SI,018D
        JE acomodo_terminado
        JMP acomodar_siguiente 

    resta_es_positivo:
        MOV SIGNOT,01H
    JMP acomodo_terminado
    
    invertir_operandos:
    MOV SIGNOT,0FFH
    MOV SI,017D   ;~MOV SI,7H     
    invertir_siguiente:
    MOV AL,NUM1[SI]
    MOV AH,NUM2[SI]
    
    MOV NUM1[SI],AH
    MOV NUM2[SI],AL
    DEC SI
    JS acomodo_terminado 
    JMP invertir_siguiente 
    
    acomodo_terminado:
    ;;   
            ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
            MOV SI,017D ;NUMERO MAS A LA IZQUIERDA             
        
                siguiente_restando:
            

                    MOV AL,NUM1[SI] ;OP_1 
                    MOV AH,NUM2[SI]
                    SUB AL,AH
                    ;;===
                        ADD AL,ENTEROST[SI]           
                        MOV ENTEROST[SI],AL
                        ;
                    ;;===
                    DEC SI
                    JNS siguiente_restando                      
                    

            ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;       
            
            ;Ajustar acarreos        
        
            MOV SI,017D  
        
            siguiente_acarreo_neg:
        
            MOV AL,ENTEROST[SI]
            CMP AL,0H 
            JS con_acarreo_neg
            JMP sin_acarreo_neg   
            ;;
            con_acarreo_neg: 
                NEG AL
                MOV AH,0AH
                SUB AH,AL
                MOV ENTEROST[SI],AH
                
                MOV AL,ENTEROST[SI-1]
                SUB AL,01H
                MOV ENTEROST[SI-1],AL
            ;;
            sin_acarreo_neg:  
            DEC SI
            JNS siguiente_acarreo_neg
            RET
    
        
    RESTA ENDP
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
;====================================[MOSTRAR_RESULTADO]==================================== 
    MOSTRAR_RESULTADO PROC NEAR
        ;REVISAR EL SIGNO
        MOV AL,[DI] ;SIGNOT
        INC DI
        CMP AL,0FFH
        JNE no_imprime_signo
            ;IMPRIME SIGNO 
            MOV DL,'-'
            CALL IMPRIME_CARACTER
            ;
        no_imprime_signo:
        ;IMPRIME ENTEROS
        MOV SI,0FFFFH
            ;MIENTRAS SEA CERO A LA IZQUIERDA EVITAR
            siguiente_numero_entero:
            INC SI 
            ;;;;;;;;;;
            CMP SI,09H ;Ultima localidad del numero entero +1
            JE un_cero_entero
            ;;;;;;;;;; 

            ;;;;;;
            MOV CX,DI
            ADD CX,SI
            MOV BX,SI      
            MOV SI,CX     
            ;;;;;;
            MOV DL,[SI];ENTEROST[SI]  
            MOV SI,BX
            
            
            CMP DL,0H 
            JE siguiente_numero_entero
            ;YA NO ES CERO A LA IZQUIERDA
            CMP SI,09H ;SI SOLO TIENE DECIMALES - Ultima localidad del numero entero +1
            JE un_cero_entero
            ;     
                imprime_enteros:
                
                
                ;;;;;;
                MOV CX,DI
                ADD CX,SI
                MOV BX,SI      
                MOV SI,CX     
                ;;;;;;
                MOV DL,[SI];ENTEROST[SI]  
                MOV SI,BX
                
                ADD DL,30H
                CALL IMPRIME_CARACTER  
                INC SI
                CMP SI,08H ;Ultimo numero entero
                JA imprime_decimales
                JMP imprime_enteros
            ; 
            un_cero_entero: 
            MOV DL,30H
            CALL IMPRIME_CARACTER
            
           imprime_decimales:
           ;REVISAR SI TIENE DECIMALES PARA IMPRIMIR EL PUNTO
            MOV SI,08H ;Ultimo numero entero
            
            siguiente_cero_decimal:
            INC SI ;incremento en 1 para entrar en el campo de los enteros
            CMP SI,017D ;Ultimo numero decimal
            JA sin_decimales 
            
            
                ;;;;;;
                MOV CX,DI
                ADD CX,SI
                MOV BX,SI      
                MOV SI,CX     
                ;;;;;;
                MOV DL,[SI];DECIMALEST[SI]  
                MOV SI,BX
                
                
            CMP DL,0H
            JE siguiente_cero_decimal
            
            ;con decimales
            MOV DL,'.'
            CALL IMPRIME_CARACTER    
            MOV SI,08H ;Ultimo numero entero -1
            
            imprime_decimal_siguiente:
            INC SI


                ;;;;;;
                MOV CX,DI
                ADD CX,SI
                MOV BX,SI      
                MOV SI,CX     
                ;;;;;;
                MOV DL,[SI];DECIMALEST[SI]  
                MOV SI,BX
                

            ADD DL,30H
            CALL IMPRIME_CARACTER
            CMP SI,017D ;Ultimo numero decimal
            JNE imprime_decimal_siguiente
            
        
    sin_decimales:    
    RET 
    MOSTRAR_RESULTADO ENDP  
;====================================[IMPRIME_CARACTER]==================================== 
    IMPRIME_CARACTER PROC NEAR
       MOV AH,02H
       INT 21H          
    RET
    IMPRIME_CARACTER ENDP
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
