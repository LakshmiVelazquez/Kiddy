<script>
import gradoService from "../../services/grado-service";
import grupoService from "../../services/grupo/grupo-service";
import { useToast } from "vue-toastification";

export default {
  data() {
    return {
      toast: useToast(),
      search: "",
      dialog: false,
      header: [
        {
          title: "Id",
          key: "id",
        },
        {
          title: "Grupo",
          key: "nombre",
        },
        {
          title: "Opciones",
          key: "options",
        },
      ],
      idGrado: "",
      grupos: [],
      bimestre: null,
      grados: [],
      grupoGuardar: {
        id: 0,
        nombre: null,
        idGrado: 0,
      },
      grupoDefault: {
        id: 0,
        nombre: null,
        idGrado: 0,
      },
      idGrupoEliminar: 0,
      dialogEliminar: false,
    };
  },
  watch: {
    idGrado: function (newId, oldId) {
      this.obtenerBimestre();
    },
  },
  methods: {
    obtenerBimestre() {
      this.grados.forEach((element) => {
        if (element.id == this.idGrado) {
          this.bimestre = element.bimestre;
        }
      });
    },
    async GetGrupos() {
      try {
        this.grupos = await grupoService.ObtenerGrupo();
      } catch (error) {
        console.error(error);
      }
    },
    async GetGrados() {
      try {
        this.grados = await gradoService.ObtenerGrados();
      } catch (error) {
        console.error(error);
      }
    },

    async AddGrupos() {
      this.grupoGuardar.idGrado = this.idGrado;
      try {
        const result = await grupoService.AgregarGrupo(this.grupoGuardar);

        if (result.data.success) {
          this.toast.success(result.data.message);
          this.dialog = false;
          this.grupoGuardar = { ...this.grupoDefault };
        } else {
          this.toast.error(result.data.message);
          this.dialog = false;
          this.grupoGuardar = { ...this.grupoDefault };
        }

        await this.GetGrupos();
      } catch (error) {
        console.error(error);
      }
    },
    async UpdateGrupo() {
      try {
        this.grupoGuardar.idGrado = this.idGrado;
        const result = await grupoService.EditarGrupo(this.grupoGuardar);
        if (result.data.success) {
          this.toast.success(result.data.message);
          this.dialog = false;
          this.grupoGuardar = { ...this.grupoDefault };
        } else {
          this.dialog = false;
          this.grupoGuardar = { ...this.grupoDefault };
          this.toast.error(result.data.message);
        }

        await this.GetGrupos();
      } catch (error) {
        console.error(error);
      }
    },
    DialogUpdate(item) {
      this.grupoGuardar = { ...item };
      this.idGrado = item.idGrado;

      this.dialog = true;
    },
    async DeleteGrupo() {
      try {
        const result = await grupoService.EliminarGrupo(this.idGrupoEliminar);

        if (result.data.success) {
          this.toast.success(result.data.message);
          this.dialogEliminar = false;
          this.idGrupoEliminar = 0;
        } else {
          this.dialogEliminar = false;
          this.idGrupoEliminar = 0;
          this.toast.error(result.data.message);
        }

        await this.GetGrupos();
      } catch (error) {
        console.error(error);
      }
    },
    CerrarModal() {
      this.grupoGuardar = { ...this.grupoDefault };
      this.idGrado = 0;
      this.dialog = false;
    },
    CerrarDelete() {
      this.idGrupoEliminar = 0;
      this.dialogEliminar = false;
    },
    AbrirModalEliminar(idGrupo) {
      this.idGrupoEliminar = idGrupo;
      this.dialogEliminar = true;
    },
  },
  async created() {
    await this.GetGrupos();
    await this.GetGrados();
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
                <v-icon color="green" size="48"> mdi mdi-account-group </v-icon>
                Grupos
              </p>
              <v-btn @click="dialog = true" icon="mdi-plus" size="40" class="ms-4">
              </v-btn>
            </div>
          </v-col>
          <v-col cols="12" sm="7">
            <v-text-field variant="outlined" v-model="search" label="Buscar">
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
            <span class="font-weight-black">Listados de Grupos</span>
          </template>
          <div class="py-5 border-top">
            <v-data-table :headers="header" :items="grupos" :search="search">
              <template #item.options="{ item }">
                <v-btn
                  color="success"
                  size="small"
                  class="ms-2"
                  icon="mdi-pencil-box-multiple-outline"
                  text="Calificación"
                  @click="DialogUpdate(item)"
                />
                <v-btn
                  color="error"
                  size="small"
                  class="ms-2"
                  icon="mdi-trash-can"
                  text="trash"
                  @click="AbrirModalEliminar(item.id)"
                />
              </template>
            </v-data-table>
          </div>
          <!-- <div class="border-top d-flex px-4 py-2 justify-end">
            <p>pagination</p>
          </div> -->
        </v-card>
      </v-col>
    </v-row>
    <v-dialog v-model="dialog" max-width="500">
      <v-card title="Agregar Grupo">
        <v-card-text class="px-4">
          <v-row class="mt-4">
            <v-col cols="12" sm="12">
              <v-text-field label="Grupo" v-model="grupoGuardar.nombre"></v-text-field>
            </v-col>
            <!-- <v-col cols="12" sm="12">
              <p>Bimestre</p>
              <v-text-field disabled title="Bimestre" :items="bimestre">{{
                bimestre
              }}</v-text-field>
            </v-col> -->
          </v-row>
        </v-card-text>
        <v-card-actions class="justify-end">
          <v-btn text="Cancelar" @click="CerrarModal"></v-btn>
          <v-btn
            v-if="grupoGuardar.id == 0"
            color="surface-variant"
            text="Guardar"
            variant="flat"
            @click="AddGrupos()"
          ></v-btn>
          <v-btn
            v-else
            color="surface-variant"
            text="Editar"
            variant="flat"
            @click="UpdateGrupo()"
          ></v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <v-dialog v-model="dialogEliminar" max-width="400">
      <v-card>
        <v-card-title class="headline">Confirmación de Eliminación</v-card-title>
        <v-card-text>
          <p>¿Estás seguro de querer eliminar este registro?</p>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="error" text @click="DeleteGrupo">Eliminar</v-btn>
          <v-btn text @click="CerrarDelete">Cancelar</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>
