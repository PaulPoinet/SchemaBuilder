export default {
  Key: 'ObjectKey', Value: 'ObjectValue',
  children: [

    { Key: 'ObjectKey', Value: 'ObjectValue' },
    { Key: 'ObjectKey', Value: 'ObjectValue'},
    {
      Key: 'ObjectKey', Value: 'ObjectValue',
      children: [
        {
          Key: 'ObjectKey', Value: 'ObjectValue',
          children: [
            { Key: 'ObjectKey', Value: 'ObjectValue' },
            { Key: 'ObjectKey', Value: 'ObjectValue' }
          ]
        },
        { Key: 'ObjectKey', Value: 'ObjectValue' },
        { Key: 'ObjectKey', Value: 'ObjectValue' },
        {
          Key: 'ObjectKey', Value: 'ObjectValue',
          children: [
            { Key: 'ObjectKey', Value: 'ObjectValue' },
            { Key: 'ObjectKey', Value: 'ObjectValue' }
          ]
        }
      ]
    },
    { Key: 'ObjectKey', Value: 'ObjectValue',
      children: [{Key: 'ObjectKey', Value: 'ObjectValue'}]

  },
  ]
}