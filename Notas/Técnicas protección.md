Técnicas de protección
==
*Técnicas de protección de datos encontradas durante el estudio
de videojuegos.*

* No tener sistema de ficheros.
* No ponerle nombre a los ficheros -> Acceso por hash del nombre.
* Codificaciones distintas en cada sección de archivos de paquete.
* Paquetes no inmediatos (punteros como múltiplos y suma).
* Borrar en seguida ficheros de memoria para difícil localización.
* Empaquetar archivos para acceso no inmediato de archivos.


* **Requisitos algoritmo óptimo**
  * Soporte para *todos* los archivos del juego.
    * Acceso rápido -> cabecera decodificada en RAM.
  * Acceso a archivos por nombre (*Ni no kuni*).
  * Codificar cabecera [XOR con string].
  * Bytes aleatorios para despistar.
  * Compresión y cifrado de archivos individuales.
