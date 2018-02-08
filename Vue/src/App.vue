<template>
  <v-app dark>

    <v-tabs v-model="active" centered fixed>
      <v-toolbar extended dark class>
    <v-btn fab dark small @click.native='showDev'><v-icon dark>add</v-icon></v-btn>
        <v-toolbar-title slot="extension" class="display-0">Schema Builder</v-toolbar-title>
      </v-toolbar>
      <v-tabs-bar ref="myTabs">
        <v-tabs-item key="tree" href='tree'>
          <v-icon>device_hub</v-icon>
          <span>&nbsp;</span> Tree
        </v-tabs-item>
        <v-tabs-slider color="white"></v-tabs-slider>
        <v-tabs-item key="customschemas" href="customschemas">
          <v-icon>widgets</v-icon>
          <span>&nbsp;</span> My Specs
        </v-tabs-item>
      </v-tabs-bar>
      <v-tabs-items>
        <v-tabs-content key="tree" id='tree'>
          <span>&nbsp;</span>
          <global-controls>
          </global-controls>
        </v-tabs-content>
        <v-tabs-content key="customschemas" id="customschemas">
          <custom-schemas>
          </custom-schemas>
        </v-tabs-content>
      </v-tabs-items>
    </v-tabs>
    <obj-properties-global v-model="myDialog">
    </obj-properties-global>
  </v-app>
</template>
<script>

import GlobalControls from './components/GlobalControls.vue'
import CustomSchemas from './components/CustomSchemas.vue'
import Data from './components/Data.js'
import ObjPropertiesGlobal from './components/ObjPropertiesGlobal.vue'
import CompilingMenu from './components/CompilingMenu.vue'

export default {
  name: 'app',
  components: {
    GlobalControls,
    ObjPropertiesGlobal,
    CustomSchemas,
    CompilingMenu

  },

  data( ) {
    return {
      tree: Data,
      active: null,
      myDialog: false,
     
    }
  },

  methods: {
    goToPropsTab( ) {
      this.active = 'customschemas'
    },

    goToTreeTab( ) {
      this.active = 'tree'
    },
    showDev() {
      Interop.showDev()
      
    }
  },

  mounted( ) {
    window.bus.$on( 'change-to-schemasTab', state => { this.goToPropsTab( ) } )
    window.bus.$on('change-to-treeTab', state => { this.goToTreeTab() })
    window.bus.$on('launch-props', state => { 
      console.log(this.myDialog)
      this.myDialog = true 
      console.log(this.myDialog)
    })
  },
}

</script>
<style>
#app {
  font-family: Menlo, Roboto, monospace;
  font-size: 10pt;
  -webkit-font-smoothing: antialiased;
  color: rgb(169, 169, 169);
}

h1,
h2 {
  font-weight: normal;
}

ul {
  list-style-type: none;
}

a {
  color: #42b983;
}

.bold {
  font-weight: bold;
}

.tabs__wrapper {
  overflow-x: hidden;
}

.makeSmall {
  font-size: 20px;
}


ul {
  padding-left: 1em;
  line-height: 1.5em;
}
</style>