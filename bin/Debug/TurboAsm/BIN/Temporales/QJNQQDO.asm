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
OVERFLOW DB 'STACK OVERFLOW!','$'
EXCEPCION_NO_CONTROLADA DB 'EXCEPCION NO CONTROLADA:','$'
;VARIABLES DEPENDIENTES DE LA PROGRAMACION

;=======>[X1-Entero]
SGN_X1 db 0
NUM_X1 db 0,0,0,0,0,0,0,0,0
DEC_X1 db 0,0,0,0,0,0,0,0,0 

;=======>[X3-Entero]
SGN_X3 db 0
NUM_X3 db 0,0,0,0,0,0,0,0,0
DEC_X3 db 0,0,0,0,0,0,0,0,0 

;=======>[AUTOVAR9f283064944a4ad2a49a1e72929a406b-Cadena]
AUTOVAR9f283064944a4ad2a49a1e72929a406b DW 00
LONGUITUD_AUTOVAR9f283064944a4ad2a49a1e72929a406b DW 0

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
;================[-CODIGO TRES DIRECCIONES-]================

;Parte Entera

 MOV SGN_X1,1H
MOV NUM_X1[8h],9
MOV NUM_X1[7h],9
MOV NUM_X1[6h],0
MOV NUM_X1[5h],0
MOV NUM_X1[4h],0
MOV NUM_X1[3h],0
MOV NUM_X1[2h],0
MOV NUM_X1[1h],0
MOV NUM_X1[0h],0

;Parte Decimal
MOV DEC_X1[0h],1
MOV DEC_X1[1h],2
MOV DEC_X1[2h],0
MOV DEC_X1[3h],0
MOV DEC_X1[4h],0
MOV DEC_X1[5h],0
MOV DEC_X1[6h],0
MOV DEC_X1[7h],0
MOV DEC_X1[8h],0
;================[-CODIGO TRES DIRECCIONES-]================
;=========[T1 = X1/-4]=========

LEA DX,SGN_X1
CALL USAR_N1  ;vamos a utilizar T_X1

;Parte Entera

 MOV SGN2,0FFH
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
CALL DIVIDE
LEA DX,SGN_SIGNOT1
CALL SALVAR_TEMPORAL

LEA DX,SGN_X3
CALL SALVAR_TEMPORAL
;================[-ALOGAR CADENA-]================
MOV BLOQUE_ACTUAL,11D
CALL ALOGAR_CADENA
MOV SI,CADENA
;=================[ASIGNAR VALOR A LA CADENA]=================
MOV DL,'R'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,'s'
MOV [SI],DL
INC SI
MOV DL,'u'
MOV [SI],DL
INC SI
MOV DL,'l'
MOV [SI],DL
INC SI
MOV DL,'t'
MOV [SI],DL
INC SI
MOV DL,'a'
MOV [SI],DL
INC SI
MOV DL,'d'
MOV [SI],DL
INC SI
MOV DL,'o'
MOV [SI],DL
INC SI
MOV DL,':'
MOV [SI],DL
INC SI
MOV DL,10
MOV [SI],DL
INC SI
MOV DL,13
MOV [SI],DL
INC SI
MOV DL,FIN_CADENA
MOV [SI],DL
MOV DX,CADENA
MOV AUTOVAR9f283064944a4ad2a49a1e72929a406b,DX
MOV DX,AUTOVAR9f283064944a4ad2a49a1e72929a406b
CALL IMPRIME_CADENA
LEA DI,SGN_X3
CALL MOSTRAR_RESULTADO
;==============>[FIN CODIGO GENERADO POR EL COMPILADOR]<==============
XOR AX,AX
INT 16H
;================>[LIBERAR TODA LA MEMORIA DE CADENAS]<================
MOV BX,AUTOVAR9f283064944a4ad2a49a1e72929a406b
CALL LIBERAR_CADENA
MOV AH,4CH
INT 21H
RET

;==============>[PROCEDIMIENTOS INTERNOS GENERADOS POR EL COMPILADOR]<==============
;====================================[DIVIDE]====================================    
    DIVIDE PROC NEAR
        MOV DIVISON[0H],0
        MOV DIVISON[2H],0
        MOV DIVISON[4H],0
        
        MOV POSICION_DIV,0H     

        ;LOGICA DE SIGNOS :) BASTANTE SENCILLO AQUI
        MOV AL,SGN1
        MOV AH,SGN2
        IMUL AH
        PUSH AX ;SALVAR SIGNO

        ;PARTE_ENTERA
        divide_siguiente:
           
        CALL RESTA   
        MOV AL,SIGNOT
        CMP AL,0FFH
        JE parte_decimal
        
        ;  
            CMP POSICION_DIV,0H
            JE enteros_div  
            JA decimales_div_
                enteros_div:
                MOV SI,POSICION_DIV 
                ADD DIVISON[SI],01H
                  
                JMP continuar_div
                decimales_div_:
                MOV SI,POSICION_DIV
                ADD DIVISON[SI],01H  
                ;JMP continuar_div
                
            continuar_div:    
            MOV SIGNOT,0H 
            LEA DX,SIGNOT
            CALL USAR_N1 
        ;
        JMP divide_siguiente
        
        
        parte_decimal:   
        ;MULTIPLICAR RESUIDO POR 10    
        MOV SI,018D    
        
        copy_next_div: 
        MOV AL,SGN1[SI]
        MOV TEMP[SI],AL
        DEC SI
        JNS copy_next_div
        
        LEA DX,SIGNOT
        CALL LIMPIAR
        
        MOV SGN1,0H
        MOV NUM1[0],0
        MOV NUM1[1],0
        MOV NUM1[2],0
        MOV NUM1[3],0 
        MOV NUM1[4],0 
        MOV NUM1[5],0 
        MOV NUM1[6],0 
        MOV NUM1[7],1 
        MOV NUM1[8],0 
        
        MOV DEC1[0],0
        MOV DEC1[1],0
        MOV DEC1[2],0
        MOV DEC1[3],0
        MOV DEC1[4],0
        MOV DEC1[5],0
        MOV DEC1[6],0
        MOV DEC1[7],0
        MOV DEC1[8],0
        
        CALL MULTIPLICA
        
        LEA DX,TEMP
        CALL USAR_N2 
        
        LEA DX,SIGNOT
        CALL USAR_N1
        
        ADD POSICION_DIV,2H
        CMP POSICION_DIV,06H
        JAE fin_division
        ;;;;;;;;;;;;;;;;;;REVISAR QUE EL NUM1 NO SEA YA CERO :V
        MOV SI,018D ;Posicion del ultimo digito decimal ~  
        revisar_siguiente_div:
            MOV AL,ENTEROST[SI]
            CMP AL,0H
            JNE divide_siguiente
        DEC SI
        CMP SI,0H
        JE fin_division
        JNE revisar_siguiente_div 
        ;;;;;;;;;;;;;;;;;;
        
        JMP divide_siguiente
        fin_division:
        ;COPIAR RESULTADO A T
            ;LIMPIAR VARIABLES
            LEA DX,SIGNOT
            CALL LIMPIAR

            MOV AX,DIVISON[0H]
            AAM
            MOV ENTEROST[7H],AH
            MOV ENTEROST[8H],AL
            
            MOV AX,DIVISON[2H]
            AAM
            MOV DECIMALEST[0H],AL
            MOV DECIMALEST[1H],AH 
            MOV AX,DIVISON[4H]
            AAM
            ADD DECIMALEST[0H],AH 
            ADD DECIMALEST[1H],AL

           POP AX 
           MOV SIGNOT,AL  ;RECUPERAR SINGO
        RET 
        
    DIVIDE ENDP
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
;====================================[MULTIPLICA]====================================
    MULTIPLICA PROC NEAR
    ;LIMPIAR VARIABLES
    LEA DX,SIGNOT
    CALL LIMPIAR
    ;LOGICA DE SIGNOS :) bastante sencillo aqui
        MOV AL,SGN1
        MOV AH,SGN2
        IMUL AH
        MOV SIGNOT,AL 
        
            MOV AUX,0H  
            ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
            MOV SI,12D ;NUMERO MAS A LA IZQUIERDA             
        
                siguiente_multiplicando:
                MOV DI,012D ;NUMERO MAS A LA IZQUIERDA
                ;MOV CX,7H 
                    MOV CX,016D
                    
                SUB CX,AUX
            
                 multiplicar_operandos:
                    MOV AL,NUM2[SI] ;OP_1  
                    MOV AH,NUM1[DI]                 
                    MUL AH
                    AAM
                    ;;===
                    MOV BX,DI
                    MOV DI,CX
                
                        ;
                        MOV DX,DI
                        MOV DI,CX
                    
                        ADD AL,ENTEROST[DI]           
                        MOV ENTEROST[DI],AL

                   
                        ADD AH,ENTEROST[DI-1]
                        MOV ENTEROST[DI-1],AH  
                    
                        MOV DI,DX
                        ;
                
                    ;;===
                    MOV DI,BX
                    DEC CX
                    ;;
                    DEC DI
                    JNZ multiplicar_operandos 
                    ;;;
                    INC AUX   
                    DEC CX
                    DEC SI
                    JNS siguiente_multiplicando                      
                    

            ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;       
            
            ;Ajustar acarreos        
        
            MOV SI,017D  
        
            siguiente_acarreo_multiplica:
        
            MOV AL,ENTEROST[SI]
            CMP AL,0AH
            JL sin_acarreo_multiplica 
            AAM
            MOV ENTEROST[SI],AL
            ADD ENTEROST[SI-1H],AH

            sin_acarreo_multiplica:  
            DEC SI
            JNS siguiente_acarreo_multiplica
            RET
        
    MULTIPLICA ENDP
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
