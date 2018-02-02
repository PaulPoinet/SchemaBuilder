<template>
  <div>
  <v-speed-dial v-model="fab" :top="top" :bottom="bottom" :right="right" :left="left" :direction="direction" :hover="hover" :transition="transition" :fixed="fixed">
    <v-btn slot="activator" color="grey darken-2" dark fab hover v-model="fab">
      <v-icon color="yellow">play_for_work</v-icon>
      <v-icon color="white">close</v-icon>
    </v-btn>
    <v-btn fab dark small color="grey darken-2">
      <v-icon color="white" >fa-cogs</v-icon>
    </v-btn>
    <v-btn @click="goCheckIfSelected()" fab dark small color="grey darken-2">
      <v-icon color="yellow" >extension</v-icon>
    </v-btn>
  </v-speed-dial>
    <v-dialog v-model="dialog0" max-width="290">
      <v-card>
        <v-card-title class="headline">
          <v-icon class="ma-2" color="yellow" light>fa-exclamation-triangle</v-icon>Selection Empty!</v-card-title>
        <v-card-text>Retry by selecting one (and only one) RhinoObject from which you want to attach the current tree as a UserDicitonary.</v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="green darken-1" flat="flat" @click="dialog0 = false">OK</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <v-dialog v-model="dialog2" max-width="290">
      <v-card>
        <v-card-title class="headline">Too many objects are selected!</v-card-title>
        <v-card-text>Retry by selecting one (and only one) RhinoObject from which you want to attach the current tree as a UserDicitonary.</v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="green darken-1" flat="flat" @click="dialog2 = false">OK</v-btn>
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

      fixed: true,
      direction: "top",
      fab: false,
      fling: false,
      hover: false,
      tabs: null,
      top: false,
      right: true,
      bottom: true,
      left: false,
      transition: 'slide-y-reverse-transition',
      dialog0: false,
      dialog2: false,
    }

  },

  methods: {
    goCheckIfSelected( ) {
      Interop.checkIfSomethingIsSelected();
    },
  },

  mounted(){

    window.bus.$on( 'one-object-selected', state => { 
      console.log("one object is selected")
    } )
    window.bus.$on( 'multiple-objects-selected', state => { 
      console.log("multiple objects are selected")
      this.$nextTick(this.dialog0 = true)
    } )

  },
  updated(){
     window.bus.$on( 'no-object-selected', state => { 
      console.log("nothing is selected")
      this.$nextTick(this.dialog0 = true)
    } )  
    window.bus.$on( 'one-object-selected', state => { 
      console.log("one object is selected")
      this.$nextTick(this.dialog1 = true)
    } ) 
    window.bus.$on( 'one-object-selected', state => { 
      console.log("one object is selected")
      this.$nextTick(this.dialog2 = true)
    } ) 
  }

}
</script>
<style>
</style>