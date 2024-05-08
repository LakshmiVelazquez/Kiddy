import { defineStore } from 'pinia';

interface AuthState {
  accessToken: string | null;
  usuario: string | null;
  tipoUsuario: string | null; // Nueva propiedad para el tipo de usuario
}

export const useAuthStore = defineStore({
  id: 'auth', // Identificador único del store
  state: (): AuthState => ({
    accessToken: null,
    usuario: null,
    tipoUsuario: null, // Inicialmente nulo
  }),
  actions: {
    setAuthData(accessToken: string, usuario: string, tipoUsuario: string) {
      this.accessToken = accessToken;
      this.usuario = usuario;
      this.tipoUsuario = tipoUsuario;
    },
    clearAuthData() {
      this.accessToken = null;
      this.usuario = null;
      this.tipoUsuario = null;
    },
  },
});