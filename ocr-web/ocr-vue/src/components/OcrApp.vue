<template>
  <v-container>
    <v-row class="text-center">
      <v-col cols="12">
        <v-img
          :src="require('../assets/logo.svg')"
          class="my-3"
          contain
          height="200"
        />
      </v-col>

      <v-col class="mb-5" cols="12">
        <h2 class="headline font-weight-bold mb-3">
          Detecção de texto em imagens
        </h2>
        <v-card-text
          >Use este serviço para extrair texto de imagens
          <br />
          Não suporta letra cursiva
        </v-card-text>
        <v-row justify="center">
          <v-file-input
            @change="handleFileSelected"
            label="Clique aqui para escolher uma imagem"
          ></v-file-input>
          <v-btn @click="handleUpload" color="primary">Enviar</v-btn>
        </v-row>
        <v-row v-if="textProcessed != ''">
          <v-col>
            <!--            <v-card-text>{{ textProcessed }}</v-card-text>-->
            <v-textarea :value="textProcessed" readonly></v-textarea>
          </v-col>
        </v-row>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import Vue from "vue";
import http from "../http-common";

export default Vue.extend({
  name: "OcrApp",

  data: () => ({
    selectedImage: null as File | null,
    textProcessed: "",
  }),
  methods: {
    handleFileSelected(file: File) {
      this.selectedImage = file;
    },

    handleUpload() {
      if (!this.selectedImage) {
        return;
      }

      let formData = new FormData();
      formData.append("image", this.selectedImage);
      console.log("carregado");
      return http
        .post("/Ocr", formData, {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        })
        .then((response) => {
          console.log("enviado com sucesso");
          console.log(response.data);
          this.textProcessed = response.data;
          console.log(this.textProcessed);
        })
        .catch((error) => {
          console.log("Erro no envio");
          console.log(`Error: ${error.message}`);
        });
    },
  },
});
</script>
