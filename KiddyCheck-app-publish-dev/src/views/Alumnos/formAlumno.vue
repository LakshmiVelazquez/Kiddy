<template>
  <v-form>
    <v-container>
      <v-row>
        <v-col cols="12" sm="6">
          <v-text-field label="Nombre" v-model="formData.nombre"></v-text-field>
        </v-col>

        <v-col cols="12" sm="6">
          <v-text-field
            label="Apellido paterno"
            v-model="formData.apellidoPaterno"
          ></v-text-field>
        </v-col>
        <v-col cols="12" sm="6">
          <v-text-field
            label="Apellido materno"
            v-model="formData.apellidoMaterno"
          ></v-text-field>
        </v-col>
        <v-col cols="12" sm="6">
          <v-select
            v-model="formData.idTutor"
            label="Tutor"
            item-value="id"
            item-title="nombreCompleto"
            :items="data.listaAlumnoTutor"
            filter
            autocomplete
          ></v-select>
        </v-col>
        <v-col cols="12" sm="6">
          <v-select
            v-model="formData.idDocente"
            label="Docente"
            item-value="id"
            item-title="nombreCompleto"
            :items="data.listaDocente"
            filter
            autocomplete
          ></v-select>
        </v-col>
        <v-col cols="12" sm="6">
          <v-select
            v-model="formData.grado"
            item-value="id"
            item-title="grado"
            label="Grado"
            :items="data.listaGrado"
          ></v-select>
        </v-col>

        <v-col cols="12" sm="6">
          <v-select
            v-model="formData.grupo"
            label="Grupo"
            item-value="id"
            item-title="nombre"
            :items="data.listaGrupo"
          ></v-select>
        </v-col>
      </v-row>
    </v-container>
  </v-form>
</template>
<script setup>
import { reactive, defineEmits, defineProps, watch, onMounted } from "vue";
import axios from "axios";
import { useToast } from "vue-toastification";

const apiURL = import.meta.env.VUE_APP_API_URL;
const toast = useToast();
const props = defineProps({
  data: Object,
});
const data = reactive({
  listaGrupo: [],
  listaGrado: [],
  listaAlumnoTutor: [],
  listaDocente: [],
});
const formData = reactive({
  nombre: "",
  apellidoPaterno: "",
  apellidoMaterno: "",
  idTipoPersona: 4,
  idTutor: "",
  correo: " ",
  grado: "",
  grupo: "",
  //"telefono": " ",
  correo: " ",
  password: " ",
});
const emit = defineEmits(["onChangeValue"]);

watch(
  () => JSON.stringify(formData),
  (newValue, oldValue) => {
    emit("onChangeValue", JSON.parse(newValue));
  }
);
async function obtenerListaTutor() {
  const url = `https://localhost:7163/api/v1/Personas/ObtenerPersonaPorTipo?id=3`;
  let response = await axios.get(url, {
    headers: {
      "Content-Type": "application/json",
    },
  });

  if (response.status == 200) {
    data.listaAlumnoTutor = response.data.data;
  } else {
    this.toast.success("Ocurrio un error");
  }
}
async function obtenerGrado() {
  const url = `https://localhost:7163/api/Grado/ObtenerGrados`;
  let response = await axios.get(url, {
    headers: {
      "Content-Type": "application/json",
    },
  });

  if (response.status == 200) {
    data.listaGrado = response.data;
  } else {
    toast.success("Ocurrio un error");
  }
}

async function obtenerGrupo() {
  const url = `https://localhost:7163/api/Grupo/ObtenerGrupos`;
  let response = await axios.get(url, {
    headers: {
      "Content-Type": "application/json",
    },
  });

  if (response.status == 200) {
    data.listaGrupo = response.data;
  } else {
    toast.success("Ocurrio un error");
  }
}

async function obtenerListaDocente() {
  const url = `https://localhost:7163/api/v1/Personas/ObtenerPersonaPorTipo?id=2`;
  let response = await axios.get(url, {
    headers: {
      "Content-Type": "application/json",
    },
  });

  if (response.status == 200) {
    data.listaDocente = response.data.data;
  } else {
    this.toast.success("Ocurrio un error");
  }
}
onMounted(() => {
  const { data } = props;
  if (data?.id > 0) {
    formData.id = data.id;
    formData.nombre = data.nombre;
    formData.apellidoPaterno = data.apellidoPaterno;
    formData.apellidoMaterno = data.apellidoMaterno;
    formData.grado = data.grado;
    formData.grupo = data.grupo;
    formData.idTutor = data.idTutor;
    formData.idDocente = data.idDocente;
  }
  obtenerGrado();
  obtenerGrupo();
  obtenerListaTutor();
  obtenerListaDocente();
});
</script>
