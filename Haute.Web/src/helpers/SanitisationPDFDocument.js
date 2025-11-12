// import React, { useEffect } from 'react';
// import { QRCode } from 'react-qr-svg';
// import JsPDF from 'jspdf';
// import html2canvas from 'html2canvas';
// import moment from 'moment';

// // const template = require('./A4SizePdf');

// const SanitisationPDFDocument = ({
//   valueForQRCode,
//   pageName,
//   valueForPDFContent,
//   stickerValue,
// }) => {
//   const bgpage = {
//     fontFamily: 'Tahoma,sans-serif',
//     boxSizing: 'border-box',
//     width: '297mm',
//     height: '210mm',
//     margin: '5mm',
//     backgroundRepeat: 'repeat',
//     display: 'none',
//   };

//   function svgString2Image(svgString, width, height, format, callback) {
//     const svgData = `${'data:image/svg+xml;base64,'}${btoa(
//       unescape(encodeURIComponent(svgString))
//     )}`;
//     const canvas = document.createElement('canvas');
//     const context = canvas.getContext('2d');
//     canvas.width = width;
//     canvas.height = height;
//     const image = new Image();
//     image.onload = function () {
//       context.clearRect(0, 0, width, height);
//       context.drawImage(image, 0, 0, width, height);
//       const pngData = canvas.toDataURL(`${'image/'}${format}`);
//       callback(pngData);
//     };
//     image.src = svgData;
//   }

//   const refs = React.createRef();
//   useEffect(() => {
//     const div = refs.current;
//     const svg = div.querySelector('svg');
//     const serializer = new XMLSerializer();

//     // const options = {
//     //     allowTaint: true,
//     //     logging: true,
//     //     useCORS: true,
//     //     scrollY: 0,
//     //     scrollX: 0,
//     //     scale: 1,
//     //     onclone: (cloneDoc) => {
//     //         const ccc = cloneDoc;
//     //         ccc.getElementsByClassName('A4SizePage')[0].style.display = 'block';
//     //         // ccc.getElementsByClassName('SANITIZEDQRCODE')[0]
//     //         const qrImg = ccc.getElementsByClassName("SANITIZEDQRCODE")[0];
//     //         qrImg.src = {}
//     //     },
//     // }

//     svgString2Image(svgStr, 70, 65, 'JPEG', function (pngData) {
//       const htmlsource = div.getElementsByClassName('A4SizePage')[0];
//       html2canvas(htmlsource, {
//         allowTaint: true,
//         logging: true,
//         useCORS: true,
//         dpi: 300,
//         scrollY: 0,
//         scrollX: 0,
//         scale: 1,
//         onclone: (cloneDoc) => {
//           const ccc = cloneDoc;
//           ccc.getElementsByClassName('A4SizePage')[0].style.display = 'block';
//           ccc.getElementsByClassName('SANITIZEDQRCODE')[0].src = pngData;
//           ccc.getElementsByClassName('DATESANITIZED')[0].innerHTML = moment(
//             valueForPDFContent.setReminder
//           ).format('YYYY/MM/DD');
//           ccc.getElementsByClassName('CANISTERREFTRACKINGNO')[0].innerHTML =
//             valueForPDFContent.id;
//           ccc.getElementsByClassName('PRODUCT')[0].innerHTML =
//             valueForPDFContent.productName;
//           ccc.getElementsByClassName('CLEAREDBY')[0].innerHTML =
//             valueForPDFContent.firstName + ' ' + valueForPDFContent.lastName;
//         },
//       }).then(function (canvas) {
//         const imgData = canvas.toDataURL('image/png');
//         const pdf = new JsPDF('l', 'mm', 'a4');
//         pdf.addImage(imgData, 'PNG', 0, 0);
//         const str = new Date().toLocaleString();
//         pdf.save(`${'HauteWebPortal_'}${pageName}_${str}.pdf`);
//       });
//     });
//     // svg.parentNode.removeChild(svg);
//   }, [valueForQRCode]);
//   return (
//     <>
//       <div ref={refs}>
//         <QRCode
//           level="Q"
//           style={{ width: 256, marginBottom: 50, display: 'none' }}
//           value={`${pageName},${valueForQRCode.toString()}`}
//         />
//         <div
//           id="A4SizePage"
//           className="A4SizePage"
//           style={bgpage}
//           marginLeft={'auto'}
//           marginRight={'auto'}
//         >
//           <section style={{ padding: '10mm 10px 0px 10mm' }}>
//             <div>
//               <div
//                 style={{
//                   height: '32mm',
//                   padding: '15px 0px 17px 25px',
//                   margin: '18px 0px',
//                   boxSizing: 'border-box',
//                   marginTop:
//                     stickerValue == 1
//                       ? '100px'
//                       : stickerValue == 2
//                       ? '200px'
//                       : stickerValue == 3
//                       ? '300px'
//                       : stickerValue == 4
//                       ? '400px'
//                       : stickerValue == 5
//                       ? '500px'
//                       : 'unset',
//                 }}
//               >
//                 <div
//                   style={{
//                     border: '5px solid #000000',
//                     background: '#ffffff',
//                     height: '100%',
//                     width: '110mm',
//                     boxSizing: 'border-box',
//                     marginLeft: 'auto',
//                     marginRight: 'auto',
//                   }}
//                 >
//                   <div
//                     style={{
//                       display: 'flex',
//                       flexWrap: 'wrap',
//                       boxSizing: 'border-box',
//                     }}
//                   >
//                     <div
//                       style={{
//                         verticalAlign: 'middle',
//                         textAlign: 'center',
//                         width: '25%',
//                         lineHeight: '50px',
//                         padding: '7px 9px',
//                         boxSizing: 'border-box',
//                       }}
//                     >
//                       <img className="SANITIZEDQRCODE" alt="" />
//                     </div>
//                     <div
//                       style={{
//                         fontSize: '13px',
//                         lineHeight: '17px',
//                         width: '75%',
//                         marginTop: '5px',
//                         color: '#000',
//                         boxSizing: 'border-box',
//                       }}
//                     >
//                       <dl style={{ margin: '0px', lineHeight: '10px' }}>
//                         <dt
//                           style={{
//                             float: 'left',
//                             clear: 'left',
//                             marginRight: '5px',
//                             boxSizing: 'border-box',
//                             fontWeight: 'normal',
//                           }}
//                         >
//                           DATE SANITIZED:
//                         </dt>
//                         <dd
//                           style={{
//                             marginLeft: '0px',
//                             boxSizing: 'border-box',
//                             fontWeight: 'normal',
//                           }}
//                           id="DATESANITIZED"
//                           className="DATESANITIZED"
//                         >
//                           none
//                         </dd>
//                         <dt
//                           style={{
//                             float: 'left',
//                             clear: 'left',
//                             marginRight: '5px',
//                             boxSizing: 'border-box',
//                             fontWeight: 'normal',
//                           }}
//                         >
//                           CANISTER REF TRACKING NO:
//                         </dt>
//                         <dd
//                           style={{
//                             marginLeft: '0px',
//                             boxSizing: 'border-box',
//                             fontWeight: 'normal',
//                           }}
//                           id="CANISTERREFTRACKINGNO"
//                           className="CANISTERREFTRACKINGNO"
//                         >
//                           none
//                         </dd>
//                         <dt
//                           style={{
//                             float: 'left',
//                             clear: 'left',
//                             marginRight: '5px',
//                             boxSizing: 'border-box',
//                             fontWeight: 'normal',
//                           }}
//                         >
//                           PRODUCT:
//                         </dt>
//                         <dd
//                           style={{
//                             marginLeft: '0px',
//                             boxSizing: 'border-box',
//                             fontWeight: 'normal',
//                           }}
//                           className="PRODUCT"
//                         >
//                           XXXXXXXXXXXXXXXX
//                         </dd>
//                         <dt
//                           style={{
//                             float: 'left',
//                             clear: 'left',
//                             marginRight: '5px',
//                             boxSizing: 'border-box',
//                             fontWeight: 'normal',
//                           }}
//                         >
//                           SECURITY CLEARED BY:
//                         </dt>
//                         <dd
//                           style={{
//                             marginLeft: '0px',
//                             boxSizing: 'border-box',
//                             fontWeight: 'normal',
//                           }}
//                           className="CLEAREDBY"
//                         >
//                           XXXXXXXXXXXXXXXX
//                         </dd>
//                       </dl>
//                     </div>
//                   </div>
//                 </div>
//               </div>
//             </div>
//           </section>
//         </div>
//       </div>
//     </>
//   );
// };
// export default SanitisationPDFDocument;

import React, { useEffect } from 'react';
import JsPDF from 'jspdf';
import QRCode from 'qrcode';
import moment from 'moment';

const SanitisationPDFDocument = ({
  valueForQRCode,
  pageName,
  valueForPDFContent,
  stickerValue,
}) => {
  const generatePDF = async () => {
    const pdf = new JsPDF('l', 'mm', 'a4');
    const qrCodeDataURL = await QRCode.toDataURL(
      `${pageName},${valueForQRCode.toString()}`,
      { scale: 10 }
    );

    pdf.setFont('Tahoma', 'normal');
    pdf.setFontSize(8);

    // Setting margins and layout
    pdf.setTextColor(0, 0, 0);
    const marginLeft = 75;
    const boxWidth = 236; // Width of the blank part
    const boxHeight = 32; // Height of each sticker row
    const qrCodeSize = 16; // Size of the QR code
    const padding = 0; // Padding inside the box

    // Calculate the center y position based on sticker value
    const qrCodeYPositions = [20, 59, 98, 136, 176];
    let qrCodeYPosition = qrCodeYPositions[stickerValue - 1];

    // Draw outer box (for debugging purposes, comment out for production)
    // pdf.setDrawColor(0);
    // pdf.setFillColor(255, 255, 255);
    // pdf.rect(marginLeft, qrCodeYPosition - 16, boxWidth, boxHeight, 'FD');

    // Add text inside the box
    let textLeft = marginLeft + padding;
    let textTop = qrCodeYPosition;

    // Add QR Code
    pdf.addImage(
      qrCodeDataURL,
      'PNG',
      marginLeft + 134,
      textTop - 4,
      qrCodeSize,
      qrCodeSize
    );

    pdf.text('DATE SANITIZED:', textLeft, textTop);
    pdf.text(
      String(moment(valueForPDFContent.setReminder).format('YYYY/MM/DD')),
      textLeft + 25,
      textTop
    );

    pdf.text('CANISTER REF TRACKING NO:', textLeft, textTop + 3);
    pdf.text(String(valueForPDFContent.id), textLeft + 42, textTop + 3);

    pdf.text('PRODUCT:', textLeft, textTop + 6);
    pdf.text(
      String(
        valueForPDFContent.canisterCode == null
          ? ''
          : valueForPDFContent.canisterCode
      ),
      textLeft + 15,
      textTop + 6
    );

    pdf.text('SECURITY CLEARED BY:', textLeft, textTop + 9);
    pdf.text(
      `${String(valueForPDFContent.firstName)} ${String(
        valueForPDFContent.lastName
      )}`,
      textLeft + 35,
      textTop + 9
    );

    // ///////////////////////////////

    // qrCodeYPosition = qrCodeYPositions[2 - 1];

    // // Draw outer box (for debugging purposes, comment out for production)
    // // pdf.setDrawColor(0);
    // // pdf.setFillColor(255, 255, 255);
    // // pdf.rect(marginLeft, qrCodeYPosition - 16, boxWidth, boxHeight, 'FD');

    // // Add text inside the box
    // textLeft = marginLeft + padding;
    // textTop = qrCodeYPosition;

    // // Add QR Code
    // pdf.addImage(
    //   qrCodeDataURL,
    //   'PNG',
    //   marginLeft + 134,
    //   textTop - 4,
    //   qrCodeSize,
    //   qrCodeSize
    // );

    // pdf.text('DATE SANITIZED:', textLeft, textTop);
    // pdf.text(
    //   String(moment(valueForPDFContent.setReminder).format('YYYY/MM/DD')),
    //   textLeft + 25,
    //   textTop
    // );

    // pdf.text('CANISTER REF TRACKING NO:', textLeft, textTop + 3);
    // pdf.text(String(valueForPDFContent.id), textLeft + 42, textTop + 3);

    // pdf.text('PRODUCT:', textLeft, textTop + 6);
    // pdf.text(
    //   String(valueForPDFContent.productName),
    //   textLeft + 15,
    //   textTop + 6
    // );

    // pdf.text('SECURITY CLEARED BY:', textLeft, textTop + 9);
    // pdf.text(
    //   `${String(valueForPDFContent.firstName)} ${String(
    //     valueForPDFContent.lastName
    //   )}`,
    //   textLeft + 35,
    //   textTop + 9
    // );

    // ///////////////////////////////

    // qrCodeYPosition = qrCodeYPositions[3 - 1];

    // // Draw outer box (for debugging purposes, comment out for production)
    // // pdf.setDrawColor(0);
    // // pdf.setFillColor(255, 255, 255);
    // // pdf.rect(marginLeft, qrCodeYPosition - 16, boxWidth, boxHeight, 'FD');

    // // Add text inside the box
    // textLeft = marginLeft + padding;
    // textTop = qrCodeYPosition;

    // // Add QR Code
    // pdf.addImage(
    //   qrCodeDataURL,
    //   'PNG',
    //   marginLeft + 134,
    //   textTop - 4,
    //   qrCodeSize,
    //   qrCodeSize
    // );

    // pdf.text('DATE SANITIZED:', textLeft, textTop);
    // pdf.text(
    //   String(moment(valueForPDFContent.setReminder).format('YYYY/MM/DD')),
    //   textLeft + 25,
    //   textTop
    // );

    // pdf.text('CANISTER REF TRACKING NO:', textLeft, textTop + 3);
    // pdf.text(String(valueForPDFContent.id), textLeft + 42, textTop + 3);

    // pdf.text('PRODUCT:', textLeft, textTop + 6);
    // pdf.text(
    //   String(valueForPDFContent.productName),
    //   textLeft + 15,
    //   textTop + 6
    // );

    // pdf.text('SECURITY CLEARED BY:', textLeft, textTop + 9);
    // pdf.text(
    //   `${String(valueForPDFContent.firstName)} ${String(
    //     valueForPDFContent.lastName
    //   )}`,
    //   textLeft + 35,
    //   textTop + 9
    // );

    // ///////////////////////////////

    // qrCodeYPosition = qrCodeYPositions[4 - 1];

    // // Draw outer box (for debugging purposes, comment out for production)
    // // pdf.setDrawColor(0);
    // // pdf.setFillColor(255, 255, 255);
    // // pdf.rect(marginLeft, qrCodeYPosition - 16, boxWidth, boxHeight, 'FD');

    // // Add text inside the box
    // textLeft = marginLeft + padding;
    // textTop = qrCodeYPosition;

    // // Add QR Code
    // pdf.addImage(
    //   qrCodeDataURL,
    //   'PNG',
    //   marginLeft + 134,
    //   textTop - 4,
    //   qrCodeSize,
    //   qrCodeSize
    // );

    // pdf.text('DATE SANITIZED:', textLeft, textTop);
    // pdf.text(
    //   String(moment(valueForPDFContent.setReminder).format('YYYY/MM/DD')),
    //   textLeft + 25,
    //   textTop
    // );

    // pdf.text('CANISTER REF TRACKING NO:', textLeft, textTop + 3);
    // pdf.text(String(valueForPDFContent.id), textLeft + 42, textTop + 3);

    // pdf.text('PRODUCT:', textLeft, textTop + 6);
    // pdf.text(
    //   String(valueForPDFContent.productName),
    //   textLeft + 15,
    //   textTop + 6
    // );

    // pdf.text('SECURITY CLEARED BY:', textLeft, textTop + 9);
    // pdf.text(
    //   `${String(valueForPDFContent.firstName)} ${String(
    //     valueForPDFContent.lastName
    //   )}`,
    //   textLeft + 35,
    //   textTop + 9
    // );

    // ///////////////////////////////

    // qrCodeYPosition = qrCodeYPositions[5 - 1];

    // // Draw outer box (for debugging purposes, comment out for production)
    // // pdf.setDrawColor(0);
    // // pdf.setFillColor(255, 255, 255);
    // // pdf.rect(marginLeft, qrCodeYPosition - 16, boxWidth, boxHeight, 'FD');

    // // Add text inside the box
    // textLeft = marginLeft + padding;
    // textTop = qrCodeYPosition;

    // // Add QR Code
    // pdf.addImage(
    //   qrCodeDataURL,
    //   'PNG',
    //   marginLeft + 134,
    //   textTop - 4,
    //   qrCodeSize,
    //   qrCodeSize
    // );

    // pdf.text('DATE SANITIZED:', textLeft, textTop);
    // pdf.text(
    //   String(moment(valueForPDFContent.setReminder).format('YYYY/MM/DD')),
    //   textLeft + 25,
    //   textTop
    // );

    // pdf.text('CANISTER REF TRACKING NO:', textLeft, textTop + 3);
    // pdf.text(String(valueForPDFContent.id), textLeft + 42, textTop + 3);

    // pdf.text('PRODUCT:', textLeft, textTop + 6);
    // pdf.text(
    //   String(valueForPDFContent.productName),
    //   textLeft + 15,
    //   textTop + 6
    // );

    // pdf.text('SECURITY CLEARED BY:', textLeft, textTop + 9);
    // pdf.text(
    //   `${String(valueForPDFContent.firstName)} ${String(
    //     valueForPDFContent.lastName
    //   )}`,
    //   textLeft + 35,
    //   textTop + 9
    // );

    // Save PDF
    const str = new Date().toLocaleString();
    pdf.save(`HauteWebPortal_${pageName}_${str}.pdf`);
  };

  useEffect(() => {
    generatePDF();
  }, [valueForQRCode]);

  return <div style={{ display: 'none' }} />;
};

export default SanitisationPDFDocument;
