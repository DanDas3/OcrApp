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
        <v-card-text>
          Use este serviço para extrair texto de imagens
          <br />
          Não suporta letra cursiva
        </v-card-text>
        <v-row justify="center">
          <v-file-input
            @change="handleFileSelected"
            label="Clique aqui para escolher uma imagem"
            multiple
          ></v-file-input>
          <v-btn @click="handleUpload" color="primary">Enviar</v-btn>
        </v-row>
        <a ref="downloadLink" :style="{ display: 'none' }"></a>
        <v-row v-if="textProcessed !== ''">
          <v-col>
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
    selectedImages: [] as File[],
    textProcessed: null as any,
  }),
  methods: {
    handleFileSelected(files: FileList) {
      // Convertendo a lista de arquivos para um array
      this.selectedImages = Array.from(files);
    },

    handleUpload() {
      if (!this.selectedImages.length) {
        return;
      }

      let formData = new FormData();
      this.selectedImages.forEach((file, index) => {
        formData.append(`images`, file); // Certifique-se de usar o mesmo nome que o esperado no controlador
      });

      http
        .post("/Ocr/ProcessImage", formData, {
          headers: {
            "Content-Type": "multipart/form-data",
          },
          responseType: "blob",
        })
        .then((response) => {
          const blob = new Blob([response.data], { type: "application/zip" });
          const url = window.URL.createObjectURL(blob);

          const downloadLink = this.$refs.downloadLink as HTMLAnchorElement;
          downloadLink.href = url;
          downloadLink.download = "Texts.zip";

          // Simula um clique no link para iniciar o download
          downloadLink.click();

          // Limpa o objeto URL
          window.URL.revokeObjectURL(url);
          // this.textProcessed = response.data;
        })
        .catch((error) => {
          console.error(`Error: ${error.message}`);
        });
    },
  },
});
</script>
