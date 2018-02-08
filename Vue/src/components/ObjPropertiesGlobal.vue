<template>
  <div>
    <v-dialog v-model="dialogBool" scrollable max-width="400px">
      <v-card>
        <v-card-title class="headline">
          <v-icon color="blue">playlist_add_check</v-icon>&nbsp;Select some properties!</v-card-title>

        <span v-if="myDict" class="pl-4">
          <kbd>Guid:</kbd>&nbsp;<code>{{myDict["ObjectId"]}}</code>
        </span>
        <br>
        <v-layout row>
          <v-flex xs8 class="pl-4">
            <v-text-field name="whatever" label="Object's Name" v-model="inputText" dark></v-text-field>
          </v-flex>
        </v-layout>

        <v-divider></v-divider>
        <v-card-text style="height: 300px;">
          <v-checkbox class="ma-0" v-for="(value, key) in myDict" v-bind:label="`${key.toString()} : ${value.toString()}`" v-model="props" v-bind:value="`${key} : ${value}`" hide-details color="white"></v-checkbox>
        </v-card-text>
        <v-card class="ma-2" color="grey darken-4">
          <v-card-text v-if="props.length > 0">
            <span v-if="props.length > 0">Selection: {{ props }}</span>
          </v-card-text>
        </v-card>
        <v-divider></v-divider>
        <v-card-actions>
          <v-tooltip bottom>
            <v-btn @click.native="attachProps" slot="activator">
              <v-icon>play_for_work</v-icon>
            </v-btn>
            <span v-if="saveSchema">Attach properties to the tree<br> and save the template in mySpecs</span>
            <span v-if="!saveSchema">Attach properties to the tree<br> without saving the template</span>
          </v-tooltip>
          <v-tooltip bottom>
            <v-icon style="cursor: pointer;" flat icon v-if="saveSchema" @click="saveTheSchema" slot="activator">save</v-icon>
            <v-icon style="cursor: pointer;" flat icon v-if="!saveSchema" @click="saveTheSchema" slot="activator" color="grey darken-1">save</v-icon>
            <span v-if="saveSchema">Click to NOT save this template<br> in "mySpecs" when attaching</span>
            <span v-if="!saveSchema">Click to save this template<br> in "mySpecs" when attaching</span>
          </v-tooltip>
          <v-spacer></v-spacer>
          <v-btn flat icon color="red" @click.native="dialogBool = false">
            <v-icon>close</v-icon>
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>
<script>
export default {
  components: {

  },

  data( ) {
    return {
      saveSchema: false,
      saveSchema: false,
      dialogBool: false,
      myDict: null,
      inputText: "MyObjectName",
      props: [ ],
      myModelID: null,
    }
  },

  computed: {
    toPrint( ) {
      console.log( "hello" )
    },
  },

  methods: {
    backToTree( ) {
      window.bus.$emit( 'change-to-treeTab', true );
    },
    saveTheSchema( ) {
      this.saveSchema = !this.saveSchema
    },

    attachProps( ) {
      this.dialogBool = false
      window.bus.$emit( 'test', [ this.myModelID, this.myDict[ "ObjectId" ], this.props, this.inputText ] )
      this.$nextTick(window.bus.$emit( 'obj-name', this.inputText ))
      this.$nextTick( this.props = [ ] ); // Refresh Dictionaries - reinitialize the values
      this.$nextTick( this.inputText = "MyObjectName" ); // Refresh Name - reinitialize the value
    },
  },
  mounted( ) {
    window.bus.$on( 'launch-props', state => {
      this.dialogBool = state[ 0 ]
      this.myModelID = state[ 2 ]
    } )

    window.bus.$on( 'get-properties', state => {
      this.myDict = JSON.parse( state );
    } )
  },
  updated(){
    window.bus.$on( 'launch-props', state => {
      this.dialogBool = state[ 0 ]
      this.myModelID = state[ 2 ]
    } )

    window.bus.$on( 'get-properties', state => {
      this.myDict = JSON.parse( state );
    } )
  }
}
</script>
<style>
code {
  font-family: monospace;
  color: white;
  background-color: #303030;
}
</style>