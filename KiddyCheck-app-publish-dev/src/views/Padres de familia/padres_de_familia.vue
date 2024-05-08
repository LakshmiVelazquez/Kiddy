<script>
// import formAlumno from "@/views/Alumnos/formAlumno.vue";
import tutorService from "../../services/maestros/maestro-service";
import { useToast } from "vue-toastification";

export default {
  components: {
    // formAlumno,
  },
  data() {
    return {
      /* <<------------------- VARIABLES ------------------->> */
      toast: useToast(),
      refFormulario: "",

      tutorDatos: {
        id: 0,
        nombre: "",
        apellidoPaterno: "",
        apellidoMaterno: "",
        direccion: "",
        telefono: "",
        correo: "",
        password: "",
        idTipoPersona: 3,
        grado: 0,
        grupo: 0,
      },

      tutorBase: {
        id: 0,
        nombre: "",
        apellidoPaterno: "",
        apellidoMaterno: "",
        direccion: "",
        telefono: "",
        correo: "",
        password: null,
        idTipoPersona: 3,
        grado: 0,
        grupo: 0,
      },

      Tutores: [],
      OpenAgregar: false,
      modalEliminar: false,
      idEliminar: 0,
      search: "",
      headers: [
        { key: "id", title: "#" },
        { key: "nombreCompleto", title: "Nombre" },
        { key: "correo", title: "Correo" },
        { key: "telefono", title: "Teléfono" },
        { key: "direccion", title: "Dirección" },
        { key: "acciones", title: "Opciones" },
      ],
      /* <<--------------------------------------------------->> */
    };
  },
  methods: {
    /* <<------------------- METODOS CRUD------------------->> */
    async _Tutores() {
      try {
        let res = await tutorService.obtenerPersonasPorTipo(3).then((data) => data);
        if (res?.data) this.Tutores = res.data;
      } catch (error) {
        return error;
      }
    },

    async _Agregar(data) {
      try {
        let res = await tutorService.agregarPersona(data).then((data) => data);
        return res;
      } catch (error) {
        return error;
      }
    },

    async _Actualizar(data) {
      try {
        let res = await tutorService.editarPersona(data).then((data) => data);
        return res;
      } catch (error) {
        return error;
      }
    },

    async _Eliminar() {
      try {
        let res = await tutorService
          .eliminarPersona(this.idEliminar)
          .then((data) => data);
        return res;
      } catch (error) {
        return error;
      }
    },

    // async _ObtenerXId() {
    //   try {
    //     let res = await tutorService.().then(data => data)
    //     if (res?.data)
    //       return res;
    //   } catch (error) {
    //     return error;
    //   }
    // }
    /* <<--------------------------------------------------->> */

    /* <<------------------- METODOS MODALES------------------->> */
    async _abrirAgregar() {
      this.tutorDatos = { ...this.tutorBase };
      this.OpenAgregar = true;
    },
    async _cerrarAgregar() {
      this.tutorDatos = { ...this.tutorBase };
      this.OpenAgregar = false;
    },

    async _abrirEditar(item) {
      this.tutorDatos = { ...item };
      this.OpenAgregar = true;
    },
    async _cerrarEditar() {
      this.tutorDatos = { ...this.tutorBase };
      this.OpenAgregar = false;
    },

    async _abrirEliminar(id) {
      this.idEliminar = id;
      this.modalEliminar = true;
    },
    async _cerrarEliminar() {
      this.modalEliminar = false;
    },
    /* <<--------------------------------------------------->> */

    /* <<------------------- METODOS VISTA------------------->> */

    async onSubmit() {
      let form = await this.$refs.refFormulario.validate();
      if (form.valid) {
        this.Guardar();
      }
    },

    async Guardar() {
      if (this.tutorDatos.id == 0) {
        let result = await this._Agregar(this.tutorDatos).then((data) => data);
        if (result?.data?.success) {
          this.toast.success(result.data.message);
          this._cerrarAgregar();
          await this._Tutores();
        } else {
          this.toast.error(result.response.data.message);
          this._cerrarAgregar();
        }
      } else {
        let result = await this._Actualizar(this.tutorDatos).then((data) => data);
        if (result?.data?.success) {
          this.toast.success(result.data.message);
          this._cerrarEditar();
          await this._Tutores();
        } else {
          this.toast.error(result.response.data.message);
          this._cerrarEditar();
        }
      }
    },

    async _EliminarRegistro() {
      try {
        let result = await this._Eliminar().then((data) => data);

        if (result?.data?.success) {
          this.toast.success(result.data.message);
          this._cerrarEliminar();
          await this._Tutores();
        } else {
          this.toast.error(result.response.data.message);
          this._cerrarEliminar();
        }
      } catch (error) {
        console.log(error);
      }
    },

    /* <<--------------------------------------------------->> */
  },
  async created() {
    this._Tutores();
  },
};
</script>

<template>
  <v-container>
    <v-row class="justify-center">
      <v-col cols="12" sm="11" md="10">
        <v-row>
          <v-col cols="12" sm="5">
            <div class="d-flex">
              <p class="text-h4">
                <v-icon color="green" size="48"> mdi mdi-human-male-boy </v-icon>
                Tutor
              </p>
              <v-btn @click="_abrirAgregar" icon="mdi-plus" size="40" class="ms-4">
              </v-btn>
            </div>
          </v-col>
          <v-col cols="12" sm="7">
            <v-text-field
              v-model="search"
              label="Search"
              prepend-inner-icon="mdi-magnify"
              variant="outlined"
              hide-details
              single-line
            >
              <template v-slot:prepend>
                <v-icon size="40" color="green">mdi mdi-magnify </v-icon>
              </template>
            </v-text-field>
          </v-col>
        </v-row>
      </v-col>

      <v-col cols="12" sm="11" md="10">
        <v-card>
          <template v-slot:title>
            <span class="font-weight-black">Datos del tutor</span>
          </template>
          <div class="py-5 border-top">
            <v-data-table :headers="headers" :items="Tutores" :search="search">
              <template #item.acciones="{ item }">
                <div class="d-flex gap-2">
                  <VBtn
                    size="35"
                    title="Editar"
                    icon="mdi-pencil"
                    color="success"
                    @click="_abrirEditar(item)"
                  >
                  </VBtn>
                  <VBtn
                    size="35"
                    class="ml-2"
                    title="Eliminar"
                    icon="mdi-delete"
                    color="error"
                    @click="_abrirEliminar(item.id)"
                  >
                  </VBtn>
                </div>
              </template>
            </v-data-table>
          </div>
          <div class="border-top d-flex px-4 py-2 justify-end"></div>
        </v-card>
      </v-col>
    </v-row>
    <v-dialog v-model="OpenAgregar" max-width="700" persistent>
      <v-card :title="tutorDatos.id == 0 ? 'Guardar tutor' : 'Editar tutor'">
        <v-card-text class="px-4">
          <v-form ref="refFormulario" @submit.prevent="() => onSubmit()">
            <v-container>
              <v-row>
                <v-col cols="12" sm="6">
                  <v-text-field v-model="tutorDatos.nombre" label="Nombre"></v-text-field>
                </v-col>
                <v-col cols="12" sm="6">
                  <v-text-field
                    v-model="tutorDatos.apellidoPaterno"
                    label="Apellido paterno"
                  ></v-text-field>
                </v-col>
                <v-col cols="12" sm="6">
                  <v-text-field
                    v-model="tutorDatos.apellidoMaterno"
                    label="Apellido materno"
                  ></v-text-field>
                </v-col>
                <v-col cols="12" sm="6">
                  <v-text-field
                    v-model="tutorDatos.direccion"
                    label="Dirección"
                  ></v-text-field>
                </v-col>
                <v-col cols="12" sm="6">
                  <v-text-field v-model="tutorDatos.correo" label="Correo"></v-text-field>
                </v-col>
                <v-col cols="12" sm="6">
                  <v-text-field
                    v-model="tutorDatos.telefono"
                    label="Numero de telefono"
                  ></v-text-field>
                </v-col>
              </v-row>
            </v-container>
            <v-card-actions class="justify-end">
              <v-btn text="Cancelar" color="error" @click="_cerrarAgregar"></v-btn>
              <v-btn color="success" text="Guardar" type="submit" variant="flat"></v-btn>
            </v-card-actions>
          </v-form>
        </v-card-text>
      </v-card>
    </v-dialog>
  </v-container>

  <v-dialog v-model="modalEliminar" max-width="500px" persistent>
    <v-card>
      <v-card-title class="d-flex justify-center mt-1 mb-1">
        ¿Deseas eliminar el registro?
      </v-card-title>
      <v-card-actions>
        <v-spacer />
        <v-btn color="error" variant="outlined" @click="_cerrarEliminar">
          Cancelar
        </v-btn>

        <v-btn color="success" variant="elevated" @click="_EliminarRegistro">
          Aceptar
        </v-btn>
        <v-spacer />
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<style scoped>
.border-top {
  border-top: 1px #d1d5db solid;
}

.border-buttom {
  border-bottom: 1px #d1d5db solid;
}
</style>
