<template>
  <v-menu open-on-click transition="slide-x-transition" bottom right color="black">
    <v-icon slot="activator" hover v-model="fab" xs1 small flat color="grey lighten-2" class="makeSmall">more_vert</v-icon>
    <v-list class="ma-0" dark dense>
      <v-list-tile>
        <v-tooltip right>
          <v-icon style="cursor: pointer;" color="grey lighten-2" class="makeSmall" v-if="!isFolder" @click="changeType" slot="activator">create_new_folder</v-icon>
          <span>Create child</span>
        </v-tooltip>
        <v-icon style="cursor: default;" color="grey darken-2" class="makeSmall" v-if="isFolder">create_new_folder</v-icon>
      </v-list-tile>
      <v-list-tile>
        <v-tooltip right>
          <v-icon style="cursor: pointer;" class="makeSmall" color="grey lighten-2" v-if="!lock" @click="changeBehaviour" slot="activator">lock_open</v-icon>
          <span>Lock</span>
        </v-tooltip>
        <v-tooltip right>
          <v-icon style="cursor: pointer;" class="makeSmall" color="grey lighten-2" v-if="lock" @click="changeBehaviour" slot="activator">lock</v-icon>
          <span>Unlock</span>
        </v-tooltip>
      </v-list-tile>
      <v-list-tile>
        <v-tooltip right>
          <v-icon style="cursor: pointer;" class="makeSmall" color="red" @click="trashThis" slot="activator">close</v-icon>
          <span>Delete</span>
        </v-tooltip>
      </v-list-tile>
      <v-list-tile>
        <obj-properties>
        </obj-properties>
      </v-list-tile>
      <v-list-tile>
        <v-tooltip right>
          <v-icon style="cursor: pointer;" class="makeSmall" color="blue" @click="nextTabEvent" slot="activator">widgets</v-icon>
          <span>Load Specs</span>
        </v-tooltip>
      </v-list-tile>
    </v-list>
  </v-menu>
</template>
<script>
import ObjProperties from './ObjProperties.vue'
export default {

  components: {
    ObjProperties
  },

  data( ) {
    return {}
  },

  methods: {
    nextTabEvent( ) {
      window.bus.$emit( 'change-to-schemasTab', true )
    },
    trashThis( ) {
      this.$emit( 'deleteMe', this.index )
      if ( this.isFolder ) this.model.children = [ ]
    },
  }

}
</script>
<style>
</style>