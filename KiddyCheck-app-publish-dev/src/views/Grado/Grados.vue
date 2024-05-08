<script>
import gradoService from "../../services/grado-service";
import { useToast } from "vue-toastification";
import "vue-toastification/dist/index.css";
export default {
  components: {},
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
          title: "Grado",
          key: "grado",
        },
        {
          title: "Bimestre",
          key: "bimestre",
        },
        {
          title: "Opciones",
          key: "options",
        },
      ],
      dialogEliminar: false,
      grados: [],
      gradoAdd: {
        id: 0,
        grado: null,
        bimestre: null,
      },
      gradoDefault: {
        id: 0,
        grado: null,
        bimestre: null,
      },
      idGradoEliminar: 0,
    };
  },
  methods: {
    async GetGrados() {
      try {
        this.grados = await gradoService.ObtenerGrados();
      } catch (error) {
        console.error(error);
      }
    },
    async AddGrado() {
      try {
        const result = await gradoService.AgregarGrados(this.gradoAdd);
        if (result.data.success) {
          this.toast.success(result.data.message);
          this.dialog = false;
          this.gradoAdd = { ...this.gradoDefault };
        } else {
          this.toast.error(result.data.message);
          this.dialog = false;
          this.gradoAdd = { ...this.gradoDefault };
        }

        await this.GetGrados();
      } catch (error) {
        console.error(error);
      }
    },
    async UpdateGrado() {
      try {
        const result = await gradoService.EditarGrados(this.gradoAdd);
        if (result.data.success) {
          this.toast.success(result.data.message);
          this.dialog = false;
          this.gradoAdd = { ...this.gradoDefault };
        } else {
          this.dialog = false;
          this.gradoAdd = { ...this.gradoDefault };
          this.toast.error(result.data.message);
        }

        await this.GetGrados();
      } catch (error) {
        console.error(error);
      }
    },
    DialogUpdate(item) {
      console.log(item);
      this.gradoAdd = { ...item };
      this.dialog = true;
    },
    CerrarModal() {
      this.gradoAdd = { ...this.gradoDefault };

      this.dialog = false;
    },
    CerrarDelete() {
      this.idGradoEliminar = 0;
      this.dialogEliminar = false;
    },
    async EliminarDialog() {
      try {
        const result = await gradoService.EliminarGrados(this.idGradoEliminar);

        if (result.data.success) {
          this.toast.success(result.data.message);
          this.dialogEliminar = false;
          this.gradoAdd = { ...this.gradoDefault };
        } else {
          this.dialogEliminar = false;
          this.gradoAdd = { ...this.gradoDefault };
          this.toast.error(result.data.message);
        }

        await this.GetGrados();
      } catch (error) {
        console.error(error);
      }
    },
    AbrirModalEliminar(idGrado) {
      this.idGradoEliminar = idGrado;
      this.dialogEliminar = true;
    },
  },
  created() {
    this.GetGrados();
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
                <v-icon color="green" size="48"> mdi mdi-circle-multiple-outline </v-icon>
                Grados
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
        <v-card class="fluid">
          <template v-slot:title>
            <span class="font-weight-black">Listados de Grados</span>
          </template>
          <div class="py-5 border-top">
            <v-data-table :headers="header" :items="grados" :search="search">
              <template #item.options="{ item }">
                <v-btn
                  size="small"
                  class="ms-2"
                  @click="DialogUpdate(item)"
                  icon="mdi-pencil-box-multiple-outline"
                  color="success"
                  text="Calificación"
                />
                <v-btn
                  @click="AbrirModalEliminar(item.id)"
                  size="small"
                  class="ms-2"
                  icon="mdi-trash-can"
                  text="trash"
                  color="error"
                />
              </template>
            </v-data-table>
          </div>
          <!-- <div class="py-5 border-top">
            <table class="fluid">
              <thead>
                <tr>
                  <th>Id</th>
                  <th>Grado</th>
                  <th>Bimestre</th>
                  <th>Opciones</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(grado, index) in grados" :key="index">
                  <td>{{ grado.id }}</td>
                  <td>{{ grado.grado }}</td>
                  <td>{{ grado.bimestre }}</td>
                  <td>
                    <v-btn icon="mdi-pencil" @click="DialogUpdate(grado)" size="40" class="ms-4" />
                    <v-btn icon="mdi-delete" @click="AbrirModalEliminar(grado.id)" size="40" class="ms-4" />
                  </td>
                </tr>
              </tbody>
            </table>
          </div> -->
          <!-- <div class="border-top d-flex px-4 py-2 justify-end">
            <p>pagination</p>
          </div> -->
        </v-card>
      </v-col>
    </v-row>
    <v-dialog v-model="dialog" max-width="500">
      <v-card title="Guardar">
        <v-card-text class="px-4">
          <v-row class="mt-4">
            <v-col cols="12" sm="12">
              <v-text-field v-model="gradoAdd.grado" label="Grado"></v-text-field>
            </v-col>

            <v-col cols="12" sm="12">
              <v-text-field
                placeholder="0"
                v-model="gradoAdd.bimestre"
                label="Bimestre"
                type="number"
              ></v-text-field>
            </v-col>
          </v-row>
        </v-card-text>
        <v-card-actions class="justify-end">
          <v-btn text="Cancelar" @click="CerrarModal"></v-btn>
          <v-btn
            v-if="gradoAdd.id == 0"
            color="surface-variant"
            text="Guardar"
            variant="flat"
            @click="AddGrado"
          ></v-btn>
          <v-btn
            v-else
            color="surface-variant"
            text="Editar"
            variant="flat"
            @click="UpdateGrado"
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
          <v-btn color="error" text @click="EliminarDialog">Eliminar</v-btn>
          <v-btn text @click="CerrarDelete">Cancelar</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>
