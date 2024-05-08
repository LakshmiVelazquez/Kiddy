<template>
  <div>
    <div class="mb">Leer Qr</div>
    <qrcode-stream @init="onInit" @detect="onDetect"></qrcode-stream>
    <v-list lines="one">
      <v-list-item
        v-for="alumno in lista"
        :key="alumno.Id"
        :title="alumno.Nombre"
      ></v-list-item>
    </v-list>
  </div>
</template>

<script>
import { defineComponent, reactive, toRefs } from "vue";
import { QrcodeStream, QrcodeCapture } from "vue-qrcode-reader";
import axios from "axios";
import { useToast } from "vue-toastification";
import { useCookie } from "../../@core/composable/useCookie";
const API = import.meta.env.VITE_API_URL;
export default defineComponent({
  name: "QrCaptureExample",
  components: {
    "qrcode-stream": QrcodeStream,
    "qrcode-capture": QrcodeCapture,
  },
  data() {
    return {
      toast: useToast(),
      lista: [], // Lista para almacenar los códigos QR leídos
      data: "", // Resultado actual
    };
  },

  methods: {
    API_URL() {
      return import.meta.env.VITE_API_URL;
    },
    //
    async enviarNotificacion(id) {
      const url = `${API}/api/v1/Alumno/EnviarCorreo?id=${id}`;
      let response = await axios.post(
        url,
        {},
        {
          headers: {
            "Content-Type": "application/json",
            authorization: `Bearer ${useCookie("accessToken").value + ""}`,
          },
        }
      );

      if (response.status == 200) {
        this.toast.success("Se capturo asistencia");
      } else {
        this.toast.success("Ocurrio un error al enviar correo");
      }
    },
    async onInit(promise) {
      // Mostrar indicador de carga

      try {
        const { capabilities } = await promise;

        // Inicialización exitosa
      } catch (error) {
        if (error.name === "NotAllowedError") {
          // El usuario denegó el permiso de acceso a la cámara
        } else if (error.name === "NotFoundError") {
          // No se encontró un dispositivo de cámara adecuado
        } else if (error.name === "NotSupportedError") {
          // La página no se sirve a través de HTTPS (o localhost)
        } else if (error.name === "NotReadableError") {
          // Es posible que la cámara ya esté en uso
        } else if (error.name === "OverconstrainedError") {
          // ¿Solicitaste la cámara frontal aunque no haya ninguna?
        } else if (error.name === "StreamApiNotSupportedError") {
          // El navegador parece carecer de funciones necesarias
        }
      } finally {
        // Ocultar indicador de carga
      }
    },
    async onDetect(promise) {
      try {
        const items = await promise;
        const item = JSON.parse(items[0].rawValue); // Decodificar el contenido del código QR
        this.lista.push(item); // Agregar a la lista
        if (item?.Id > 0) {
          this.enviarNotificacion(item?.Id);
        }
      } catch (error) {
        // Manejar errores
      }
    },
  },
});
</script>
