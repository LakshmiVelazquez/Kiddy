<template>
  <v-container>
    {{ baseUrl }}
    <v-row class="justify-center">
      <v-col cols="12" sm="6">
        <div class="d-flex">
          <p class="text-h4">
            <v-icon color="green" size="48"> mdi mdi-account-box </v-icon>
            Alumnos
          </p>
        </div>
      </v-col>
      <v-col cols="12" sm="6">
        <v-text-field variant="outlined" v-model="search" label="Buscar">
          <template v-slot:prepend>
            <v-icon size="40" color="green">mdi mdi-magnify </v-icon>
          </template>
        </v-text-field>
      </v-col>
    </v-row>
    <v-row class="justify-end">
      <v-col cols="auto">
        <v-btn
          color="blue-grey-darken-3"
          variant="outlined"
          @click="modalFormAlumno = true"
        >
          Registra alumno
          <v-icon class="ms-2"> mdi-plus</v-icon>
        </v-btn>
      </v-col>
      <v-col cols="auto">
        <v-btn color="info" variant="outlined" @click="capturaqr = true">
          Pasar lista
          <span class="mdi mdi-order-bool-ascending-variant ms-2"></span>
        </v-btn>
      </v-col>
      <v-col cols="auto">
        <v-btn color="#227447" @click="modalExportar = true" variant="outlined">
          Exportar asistencia hoy
          <span class="mdi mdi-file-excel-outline ms-2"></span>
        </v-btn>
      </v-col>
    </v-row>
    <v-row class="justify-center">
      <v-col cols="12" sm="12" lg="12">
        <v-data-table :headers="header" :items="listaAlumno" :search="search">
          <template #item.options="{ item }" style="padding-left: 0; padding-right: 0">
            <div class="my-1">
              <v-btn
                size="35"
                class="mb-1"
                icon="mdi-qrcode"
                text="QR"
                @click="generateQrCode(item)"
              />

              <v-btn
                size="35"
                class="ms-1 mb-1"
                icon="mdi-star text-white"
                color="yellow-darken-1"
                text="Calificación"
                @click="
                  () => {
                    dataAlumno = item;
                    modalCalificacion = true;
                  }
                "
              />

              <v-btn
                size="35"
                class="ms-1 mb-1"
                icon="mdi-pencil"
                color="success"
                text="Editar"
                @click="cargarDatosEnFormulario(item)"
              />
            </div>
          </template>
        </v-data-table>
      </v-col>
    </v-row>
    <v-dialog v-model="modalFormAlumno" max-width="700">
      <v-card title="Guardar alumno">
        <v-card-text class="px-4">
          <formAlumno
            :data="dataAlumno"
            @onChangeValue="
              (v) => {
                dataAlumno = v;
              }
            "
          />
        </v-card-text>
        <v-card-actions class="justify-end">
          <v-btn text="Cancelar" @click="() => {
            (modalFormAlumno = false), (dataAlumno = {})
          }"></v-btn>
          <v-btn
            color="surface-variant"
            text="Guardar"
            variant="flat"
            @click="guardarAlumno"
          ></v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="capturaqr" max-width="500" scrollable>
      <v-card title="Asistencia">
        <v-card-text class="px-4">
          <qr-capture-alumno />
        </v-card-text>
        <v-card-actions class="justify-end">
          <v-btn text="Cerrar" @click="capturaqr = false"></v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="modalGeneraQr" max-width="400">
      <v-card title="QR asistencia">
        <v-card-text class="px-4 text-center">
          <img :src="`data:image/png;base64,${text}`" />
        </v-card-text>
        <v-card-actions class="justify-end">
          <v-btn text="Cancelar" @click="modalGeneraQr = false"></v-btn>
          <v-btn
            color="surface-variant"
            text="Imprimir"
            variant="flat"
            @click="generateQRAndPrint"
          ></v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="modalCalificacion" max-width="700">
      <v-card title="Calificaciones">
        <v-card-text class="px-4 text-center" style="height: 600px">
          <calificacion :data="dataAlumno" />
        </v-card-text>
        <v-card-actions class="justify-end">
          <v-btn
            text="Cerrar"
            @click="
              () => {
                (modalCalificacion = false), (dataAlumno = {});
              }
            "
          />
        </v-card-actions>
      </v-card>
    </v-dialog>
    <v-dialog v-model="modalExportar" max-width="400">
      <v-card title="Exportar asistencia hoy">
        <v-card-text class="px-4 text-center" style="height: 300px">
          <v-row>
            <v-col cols="12" sm="12">
              <v-select
                v-model="formExportar.grado"
                item-value="id"
                item-title="grado"
                label="Grado"
                :items="listaGrado"
              ></v-select>
            </v-col>
            <v-col cols="12" sm="12">
              <v-select
                v-model="formExportar.grupo"
                label="Grupo"
                item-value="id"
                item-title="nombre"
                :items="listaGrupo"
              ></v-select>
            </v-col>
          </v-row>
        </v-card-text>
        <v-card-actions class="justify-end">
          <v-btn text="Cerrar" @click="modalExportar = false" />
          <download-excel
            :fields="json_fields"
            :data="json_data"
            :fetch="exportarAsistencia"
          >
            <v-btn color="surface-variant" variant="flat" text="Exportar" />
          </download-excel>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>
<script>
import formAlumno from "@/views/Alumnos/formAlumno.vue";
import QrCaptureAlumno from "@/views/Alumnos/QrCaptureAlumno.vue";
import Calificacion from "@/views/Alumnos/Calificacion.vue";
import VueJsonExcel from "vue-json-excel3";
import { useCookie } from "../../@core/composable/useCookie";
import axios from "axios";
import { useToast } from "vue-toastification";

export default {
  components: {
    formAlumno,
    QrCaptureAlumno,
    Calificacion,
    "download-excel": VueJsonExcel,
  },
  computed: {
    API_URL() {
      return import.meta.env.VITE_API_URL;
    },
    imageUrl() {
      // Crear una URL Blob a partir de la cadena Base64
      const blob = this.base64ToBlob(this.base64String);
      return URL.createObjectURL(blob);
    },
  },
  data() {
    return {
      toast: useToast(),
      search: "",
      formExportar: {},
      dataAlumno: {},
      listaGrupo: [],
      listaGrado: [],
      capturaqr: false,
      modalGeneraQr: false,
      modalCalificacion: false,
      modalExportar: false,
      qrCodeData: null,
      text: "",
      options: {
        typeNumber: 4,
        errorCorrectionLevel: "M",
      },
      modalFormAlumno: false,
      listaAlumno: [],
      header: [
        {
          title: "Id",
          key: "id",
        },
        {
          title: "Nombre",
          key: "nombreCompleto",
        },
        {
          title: "Grado",
          key: "nombreGrado",
        },
        {
          title: "Grupo",
          key: "nombreGrupo",
        },
        {
          title: "Tutor",
          key: "nombreTutor",
        },
        {
          title: "Docente",
          key: "nombreDocente",
        },
        {
          title: "Opciones",
          key: "options",
        },
      ],
      json_fields: {
        Alumno: "nombreAlumno",
        Grado: "grado",
        Grupo: "grupo",
        Asistencia: "fechaAsistencia",
      },
    };
  },
  methods: {
    cargarDatosEnFormulario(item) {
      console.log(item);
      this.dataAlumno = item;
      this.modalFormAlumno = true;
    },
    async guardarAlumno() {
      const edit = this.dataAlumno?.id > 0 ? true : false;
      // Define the URL of the server endpoint you want to send the POST request to
      let url = `${this.API_URL}/api/v1/Personas/AgregarPersona`;
      if (edit) url = `${this.API_URL}/api/v1/Personas/ActualizarPersona`;

      let response = await axios.post(url, this.dataAlumno, {
        headers: {
          "Content-Type": "application/json",
          authorization: `Bearer ${useCookie("accessToken").value + ""}`,
        },
      });

      if (response.status == 200) {
        this.getListaAlumno();
        this.modalFormAlumno = false;
        this.dataAlumno = {};
        if (edit) {
          this.toast.success("Se editaron los datos del alumno correctamente");
        } else {
          this.toast.success("Se registro el alumno correctamente");
        }
      } else {
        this.toast.success("Ocurrio un error");
      }
    },

    async getListaAlumno() {
      const url = `${this.API_URL}/api/v1/Personas/ObtenerPersonaPorTipo?id=4`;
      let response = await axios.get(url, {
        headers: {
          "Content-Type": "application/json",
          authorization: `Bearer ${useCookie("accessToken").value + ""}`,
        },
      });

      if (response.status == 200) {
        this.listaAlumno = response.data.data;
      } else {
        this.toast.success("Ocurrio un error");
      }
    },
    handleDecode(decodedData) {
      this.qrCodeData = decodedData;
    },
    generateQrCode(alumno) {
      const url = `${this.API_URL}/api/v1/Alumno/generateqr?id=${alumno.id}&nombre=${alumno.nombreCompleto}`;

      // Options for the fetch request
      const options = {
        method: "POST", // HTTP method
        headers: {
          "Content-Type": "application/json", // Tell the server that the data is in JSON format
        }, // Convert the data to a JSON string
      };

      // Send the POST request
      fetch(url, options)
        .then((response) => {
          if (!response?.error) {
            return response.json();
          } else {
            throw new Error(`Network response was not ok: ${response?.error}`);
          }
        })
        .then((response) => {
          // Handle the response data
          this.text = response.data;
          this.modalGeneraQr = true;
        })
        .catch((error) => {
          // Handle any errors that occurred during the fetch
          console.error("Fetch error:", error);
        });
    },
    generateQRAndPrint() {
      // Generar la cadena base64 del código QR
      const qrCodeBase64 = `data:image/png;base64, ${this.text}`;

      // Abrir una nueva ventana
      const printWindow = window.open("", "_blank");

      // Contenido HTML para mostrar la imagen del código QR en la nueva ventana
      const htmlContent = `
        <html>
        <head>
          <title>Imprimir Código QR</title>
        </head>
        <body style="text-align: center;">
          <img src="${qrCodeBase64}" style="width: 300px; height: 300px">

        </body>
        </html>
      `;

      // Escribir el HTML en la nueva ventana
      printWindow.document.write(htmlContent);
    },
    async obtenerGrado() {
      const url = `${this.API_URL}/api/Grado/ObtenerGrados`;
      let response = await axios.get(url, {
        headers: {
          "Content-Type": "application/json",
        },
      });

      if (response.status == 200) {
        this.listaGrado = response.data;
      } else {
        toast.success("Ocurrio un error");
      }
    },

    async obtenerGrupo() {
      const url = `${this.API_URL}/api/Grupo/ObtenerGrupos`;
      let response = await axios.get(url, {
        headers: {
          "Content-Type": "application/json",
        },
      });

      if (response.status == 200) {
        this.listaGrupo = response.data;
      } else {
        toast.success("Ocurrio un error");
      }
    },

    async exportarAsistencia() {
      console.log(this.formExportar);
      const url = `${this.API_URL}/api/v1/Alumno/Exportar?idGrupo=${this.formExportar.grupo}&IdGrado=${this.formExportar.grado}`;
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
        return response.data;
      } else {
        toast.success("Ocurrio un error");
      }
      return [];
    },
  },
  created() {
    this.getListaAlumno();
    this.obtenerGrado();
    this.obtenerGrupo();
  },
};
</script>

<style scoped>
.border-top {
  border-top: 1px #d1d5db solid;
}

.border-buttom {
  border-bottom: 1px #d1d5db solid;
}
</style>
