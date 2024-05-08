<template>
  <div>
    <v-form>
      <v-row>
        <v-col cols="12" sm="6" md="4">
          <v-select v-model="formCalificacion.materia" item-value="id" item-title="nombre" label="Materia"
            :items="data.listaMateria" required></v-select>
        </v-col>
        <v-col cols="12" sm="6" md="4">
          <v-select v-model="formCalificacion.numeroBimestre" label="Bimestre a calificar" :items="listaParcial"
            required></v-select>
        </v-col>
        <v-col cols="12" sm="6" md="4">
          <v-text-field v-model="formCalificacion.calificacion" label="Calificacion" type="number" max="10" required
            min="0"></v-text-field>
        </v-col>
        <v-col cols="12" sm="6" md="4" class="align-self-center">
          <v-btn @click="GuardarCalificacion" class="float-left" color="surface-variant" text="Guardar"
            variant="outlined" :disabled="!formularioValido"></v-btn>
        </v-col>
        <v-col>
          <v-btn @click="mostrarInformacion" icon outlined>
            <v-icon>mdi-information</v-icon>
          </v-btn>
        </v-col>
      </v-row>
    </v-form>

    <v-row>
      <v-col cols="12" md="12">
        <v-divider class="border-opacity-50"></v-divider>
      </v-col>
      <v-col cols="12" sm="6">
        <v-select label="Calificación por bimestre" v-model="data.bimestreACalificar" :items="listaParcial"></v-select>
      </v-col>
      <v-col cols="12" sm="11" md="10">
        <v-table>
          <thead>
            <tr>
              <th class="text-left">Materia</th>
              <th class="text-left">Calificacion</th>
              <th class="text-left">Opciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(evaluacion, index) in data.calificacionBimestre" :key="index">
              <td>{{ evaluacion.materia }}</td>
              <td>{{ evaluacion.calificacion }}</td>
              <td>
                <v-btn icon="mdi-pencil" @click="ModalEditarCal(evaluacion.idCalificacion)" size="40" class="ms-4" />
                <v-btn icon="mdi-delete" @click="ModalEliminarCal(evaluacion.idCalificacion)" size="40" class="ms-4" />
              </td>
            </tr>
          </tbody>
        </v-table>
      </v-col>
    </v-row>
    <v-dialog v-model="data.editardialog" max-width="500">
      <v-card elevation="12">
        <v-card-title class="text-h6">Editar Calificación</v-card-title>
        <v-card-text>
          <v-row align="center" justify="center">
            <v-col cols="12" md="8">
              <v-text-field v-model="data.calificacionEdit.calificacion" label="Calificación" type="number" max="10"
                required min="0"></v-text-field>
            </v-col>
            <v-col cols="0">
              <v-row justify="center">
                <v-btn @click="EditarCalificacion" outlined class="mb-4">Editar</v-btn>
                <v-btn @click="() => {
                  data.editardialog = false
                  data.calificacionEdit.calificacion = 0
                }" outlined>Cerrar</v-btn>
              </v-row>
            </v-col>
          </v-row>
        </v-card-text>
      </v-card>
    </v-dialog>
    <v-dialog v-model="data.eliminarDialog" max-width="500">
      <v-card>
        <v-card-title class="headline">Confirmación de Eliminación</v-card-title>
        <v-card-text>
          <p>¿Estás seguro de querer eliminar este registro?</p>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="error" text @click="EliminarCal">Eliminar</v-btn>
          <v-btn text @click="() => {
            data.idEliminarCal = 0
            data.eliminarDialog = false
          }">Cancelar</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <v-dialog v-model="data.infoDialog">
      <v-card class="bg-light">
        <v-col cols="12" md="12">
          <v-table class="table table-striped table-bordered">
            <thead class="bg-light text-dark">
              <tr>
                <th class="text-left">Nombre</th>
                <th class="text-left">Apellido Paterno</th>
                <th class="text-left">Apellido Materno</th>
                <th class="text-left">Grado</th>
                <th class="text-left">Grupo</th>
                <th class="text-left">Probabilidad de pasar</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(evaluacion, index) in data.probabilidadAlumno.data" :key="index">
                <td>{{ evaluacion.nombre }}</td>
                <td>{{ evaluacion.apellidoPaterno }}</td>
                <td>{{ evaluacion.apellidoMaterno }}</td>
                <td>{{ evaluacion.grado }}</td>
                <td>{{ evaluacion.grupo }}</td>
                <td>{{ evaluacion.probabilidad }}</td>
              </tr>
            </tbody>
          </v-table>
        </v-col>
        <v-col class="text-center mt-4">
          <v-btn class="btn btn-outline-secondary" text @click="data.infoDialog = false">Cerrar</v-btn>
        </v-col>
      </v-card>
    </v-dialog>

  </div>
</template>

<script setup>
import gradoService from "../../services/grado-service";
import { reactive, defineEmits, defineProps, watch, onMounted, computed } from "vue";
import axios from "axios";
import { useToast } from "vue-toastification";
import Calificacion from '@/views/Alumnos/Calificacion.vue';
const toast = useToast();
const props = defineProps({
  data: Object,
});
// watch(
//   data.bimestreACalificar = function (newVal, OldVal) {

//   }
// )
const data = reactive({
  datoAlumno: {},
  infoDialog: false,
  listaMateria: [],
  numBimestre: 0,
  editardialog: false,
  idEliminarCal: 0,
  eliminarDialog: false,
  bimestreACalificar: 0,
  calificacionEdit: {
    idCalificacion: 0,
    materia: 'string',
    calificacion: 0,
  },
  calificacionBimestre: [],
  guardarCalificacion: {
    id: 0,
    idAlumno: 0,
    idMateria: 0,
    bimestre: '',
    calificacion: 0
  },
  resetCalificacion: {
    id: 0,
    idAlumno: 0,
    idMateria: 0,
    bimestre: '',
    calificacion: 0
  },
  probabilidadAlumno: []
});
watch(() => data.bimestreACalificar, async (newValue, oldValue) => {
  if (newValue) {
    await ObtenerCalificacionPorBimestre();
  }
});
const formCalificacion = reactive({});
const emit = defineEmits(["onChangeValue"]);

const listaParcial = computed(() => {

  let listaParcial = [];
  const numeroParcial = data.numBimestre;
  let iterableIndex = 1;
  while (iterableIndex <= numeroParcial) {
    listaParcial.push(iterableIndex);
    iterableIndex++;
  }
  return listaParcial;
});
const formularioValido = computed(() => {
  return formCalificacion.materia && formCalificacion.numeroBimestre && formCalificacion.calificacion;
});
async function ObtenerCalificacionPorBimestre() {

  try {
    const resul = await axios.get(`https://localhost:7163/api/CalificacionAlumnos/ObtenerAlumnoPorIdYBimestre?id=${props.data.id}&bimestre=${String(data.bimestreACalificar)}`,)
    data.calificacionBimestre = [...resul.data]
  } catch (error) {
    console.error(error);
  }

}
async function obtenerMateria() {
  const { grado } = props.data;
  const url = `https://localhost:7163/api/v1/Materias/MateriasXGrado?id=${grado}`;
  let response = await axios.get(url, {
    headers: {
      "Content-Type": "application/json",
    },
  });
  GetGrados()
  if (response.status == 200) {
    data.listaMateria = response.data.data;
  } else {
    this.toast.success("Ocurrio un error");
  }
}
async function GetGrados() {
  try {
    const { bimestre } = await gradoService.ObtenerGradoId(props.data.grado);
    data.numBimestre = bimestre
  } catch (error) {
    console.error(error);
  }
}
async function GuardarCalificacion() {
  try {
    data.guardarCalificacion.idMateria = formCalificacion.materia
    data.guardarCalificacion.calificacion = formCalificacion.calificacion
    data.guardarCalificacion.bimestre = String(formCalificacion.numeroBimestre)
    data.guardarCalificacion.idAlumno = props.data.id
    const resul = await axios.post('https://localhost:7163/api/CalificacionAlumnos/AgregarCalificacion', data.guardarCalificacion)
    if (resul.data.success) {
      toast.success(resul.data.message)
      data.guardarCalificacion = { ...data.resetCalificacion }
      formCalificacion.materia = null
      formCalificacion.calificacion = null
      formCalificacion.numeroBimestre = null
      await ObtenerCalificacionPorBimestre();
    }
  } catch (error) {
    console.error(error);
  }
}
async function ModalEditarCal(IdCalificacion) {
  data.editardialog = true
  data.calificacionEdit.idCalificacion = IdCalificacion
}
async function EditarCalificacion() {
  try {

    const resul = await axios.put('https://localhost:7163/api/CalificacionAlumnos/EditarCalificacion', data.calificacionEdit)
    if (resul.data.success) {
      toast.success(resul.data.message)
      data.editardialog = false
      data.calificacionEdit.calificacion = 0
      await ObtenerCalificacionPorBimestre();
    } else {
      toast.error(resul.data.message)
    }

  } catch (error) {
    console.error(error);
  }

}
async function ModalEliminarCal(id) {
  data.eliminarDialog = true
  data.idEliminarCal = id
}
async function EliminarCal() {
  try {

    const resul = await axios.delete(`https://localhost:7163/api/CalificacionAlumnos/EliminarCalificasion?id=${data.idEliminarCal}`)
    if (resul.data.success) {
      toast.success(resul.data.message)
      data.eliminarDialog = false
      data.idEliminarCal = 0
      await ObtenerCalificacionPorBimestre();
    } else {
      toast.error(resul.data.message)
    }

  } catch (error) {
    console.error(error);
  }

}
async function mostrarInformacion() {
  try {

    const resul = await axios.get(`https://localhost:7163/api/CalificacionAlumnos/ProbabilidadPorId?id=${props.data.id}`)
    data.probabilidadAlumno = resul

    data.infoDialog = true
  } catch (error) {
    console.error(error);
  }
}
onMounted(() => {
  obtenerMateria();

});
</script>
