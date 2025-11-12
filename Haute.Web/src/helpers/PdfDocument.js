import React, { useEffect } from 'react';
import { QRCode } from 'react-qr-svg';
import JsPDF from 'jspdf';
import html2canvas from 'html2canvas';
import moment from 'moment';
// import moment from "moment";

// const template = require('./A4SizePdf');

const PdfDocument = ({
  valueForQRCode,
  pageName,
  valueForPDFContent,
  clearRefillScanner,
}) => {
  // const bgpage = {
  //     fontFamily: 'Tahoma,sans-serif', boxSizing: 'border-box', width: "297mm",
  //     background: `url("http://createyourformula.hautecustombeauty.com/assets/img/pdf/hcbmdfl_logo.png")`
  //     , height: '210mm', margin: '5mm', backgroundRepeat: 'repeat', display: 'none'
  // }

  function svgString2Image(svgString, width, height, format, callback) {
    const svgData = `${'data:image/svg+xml;base64,'}${btoa(
      unescape(encodeURIComponent(svgString))
    )}`;
    const canvas = document.createElement('canvas');
    const context = canvas.getContext('2d');
    canvas.width = width;
    canvas.height = height;
    const image = new Image();
    image.onload = function () {
      context.clearRect(0, 0, width, height);
      context.drawImage(image, 0, 0, width, height);
      const pngData = canvas.toDataURL(`${'image/'}${format}`);
      callback(pngData);
    };
    image.src = svgData;
  }

  const refs = React.createRef();
  useEffect(() => {
    const div = refs.current;
    const svg = div.querySelector('svg');
    const serializer = new XMLSerializer();
    const svgStr = serializer.serializeToString(svg);
    // const options = {
    //     allowTaint: true,
    //     logging: true,
    //     useCORS: true,
    //     scrollY: 0,
    //     scrollX: 0,
    //     scale: 1,
    //     onclone: (cloneDoc) => {
    //         const ccc = cloneDoc;
    //         ccc.getElementsByClassName('A4SizePage')[0].style.display = 'block';
    //         // ccc.getElementsByClassName('SANITIZEDQRCODE')[0]
    //         const qrImg = ccc.getElementsByClassName("SANITIZEDQRCODE")[0];
    //         qrImg.src = {}
    //     },
    // }
    svgString2Image(svgStr, 100, 100, 'JPEG', function (pngData) {
      const htmlsource = div.getElementsByClassName('A4SizePage')[0];
      html2canvas(htmlsource, {
        allowTaint: true,
        logging: true,
        useCORS: true,
        dpi: 300,
        scrollY: 0,
        scrollX: 0,
        scale: 1,
        onclone: (cloneDoc) => {
          const ccc = cloneDoc;
          ccc.getElementsByClassName('A4SizePage')[0].style.display = 'block';
          ccc.getElementsByClassName('QRCODE')[0].src = pngData;
          ccc.getElementsByClassName('LOTNo')[0].innerHTML =
            valueForPDFContent.lotNr ? valueForPDFContent.lotNr : '-';
          // let code = valueForPDFContent.canisterCode
          //   .trim()
          //   .split(valueForPDFContent.canisterCode.trim().match(/[a-zA-Z]+/g));
          ccc.getElementsByClassName('RefillID')[0].innerHTML =
            valueForPDFContent.canisterCode.trim();
          // ccc.getElementsByClassName('RefillID')[0].innerHTML = code[1];
          ccc.getElementsByClassName('DATE')[0].innerHTML = moment(
            valueForPDFContent.Date
          ).format('DD/MM/YYYY');
          ccc.getElementsByClassName('PRODUCT')[0].innerHTML =
            valueForPDFContent.productName
              ? valueForPDFContent.productName
              : '';
          ccc.getElementsByClassName(
            'CANISTERCODE'
          )[0].innerHTML = `<span style="font-weight:bold"></span> <br> ${valueForPDFContent.canisterCode}`;
          ccc.getElementsByClassName(
            'CANISTERNAME'
          )[0].innerHTML = `<span style="font-weight:bold"></span> <br> ${valueForPDFContent.name}`;
          ccc.getElementsByClassName(
            'CANISTERSKU'
          )[0].innerHTML = `<span style="font-weight:bold"></span> <br> ${
            valueForPDFContent.sku ? valueForPDFContent.sku : '-'
          }`;
          ccc.getElementsByClassName('RefillBagAmount')[0].innerHTML =
            valueForPDFContent.quantity + 'ml';
        },
      }).then(function (canvas) {
        const imgData = canvas.toDataURL('image/png');
        const pdf = new JsPDF('l', 'mm', 'a5');
        pdf.addImage(imgData, 'PNG', 0, 0);
        const str = new Date().toLocaleString();
        pdf.save(`${'HauteWebPortal_'}${pageName}_${str}.pdf`);
        clearRefillScanner();
      });
    });
    // svg.parentNode.removeChild(svg);
  }, [valueForQRCode]);
  return (
    <>
      <div ref={refs}>
        <QRCode
          level="Q"
          style={{ width: 256, display: 'none' }}
          value={`${pageName},${valueForQRCode.toString()}`}
        />
        <div
          style={{
            marginLeft: '100px',
            fontFamily: 'Tahoma, sans-serif',
            lineHeight: '20px',
            // position: 'absolute',
            // zIndex: '9999999999999',
            display: 'none',
          }}
          className="A4SizePage"
        >
          <div
            style={{
              width: '210mm',
              height: '148mm',
              // background: `url("http://createyourformula.hautecustombeauty.com/assets/img/pdf/hcbmdfl_logo.png")`,
              backgroundColor: 'white',
            }}
          >
            <section
              style={{
                padding: '8mm 11mm',
                display: 'flex',
                position: 'relative',
              }}
            >
              {/* <div
                style={{
                  display: 'flex',
                  flexWrap: 'wrap',
                  margin: '0px -10px 0px -10px',
                }}
              > */}
              <div
                style={{
                  paddingRight: '40px',
                  // borderRight: '2px dashed ',
                }}
              >
                <div
                  style={{
                    // background: `url("http://createyourformula.hautecustombeauty.com/assets/img/pdf/bg_color_image_circle.png")`,
                    backgroundRepeat: 'no-repeat',
                    backgroundSize: 'contain',
                    height: '77mm',
                    width: '66mm',
                    // border: '2px solid  #f46ebd',
                    marginTop: '90px',
                    // marginLeft: '10mm',
                  }}
                >
                  <div style={{ padding: '0px 20px 0px 20px' }}>
                    <div
                      style={{
                        width: '105px',
                        fontSize: '50px',
                        textAlign: 'center',
                      }}
                    />

                    <div
                      style={{
                        textAlign: 'center',
                        padding: '10px 0px 0px 0px',
                        fontSize: '35px',
                        marginTop: '50px',
                      }}
                      className="RefillID"
                    ></div>
                    <div
                      style={{
                        fontSize: '7.5pt',
                        marginTop: '12px',
                        lineHeight: '8px',
                        padding: '0px 0px 0px 0px',
                      }}
                    >
                      <dl>
                        {/* <dt
                          style={{
                            float: ' left',
                            // clear: ' left',
                            marginRight: '5px',
                            fontWeight: 'normal',
                            fontSize: '14px',
                            padding: '5px',
                            fontWeight: 'bold',
                          }}
                        >
                          Lot:
                        </dt> */}
                        <dd
                          style={{
                            padding: '5px',
                            fontSize: '14px',
                            marginLeft: '40px',
                          }}
                          className="LOTNo"
                        >
                          XXXXXX
                        </dd>
                        {/* <dt
                          style={{
                            float: ' left',
                            // clear: ' left',
                            marginRight: '5px',
                            fontWeight: 'normal',
                            fontSize: '14px',
                            padding: '5px',
                            fontWeight: 'bold',
                          }}
                        >
                          Date:
                        </dt> */}
                        <dd
                          style={{
                            padding: '5px',
                            fontSize: '14px',
                            marginLeft: '50px',
                          }}
                          className="DATE"
                        >
                          DD/MM/YYYY
                        </dd>
                        {/* <dt
                          style={{
                            float: ' left',
                            // clear: ' left',
                            marginRight: '5px',
                            fontWeight: 'normal',
                            fontSize: '14px',
                            padding: '5px',
                            fontWeight: 'bold',
                          }}
                        >
                          Product:
                        </dt> */}
                        <dd
                          style={{
                            padding: '5px',
                            fontSize: '14px',
                            marginLeft: '65px',
                          }}
                          className="PRODUCT"
                        >
                          XXXXX
                        </dd>
                      </dl>
                    </div>
                    <div
                      style={{
                        textAlign: 'center',
                        padding: '0px 0px 0px 0px',
                        fontSize: '30px',
                        letterSpacing: '2PX',
                      }}
                      className="RefillBagAmount"
                    >
                      XXml
                    </div>
                    <div
                      style={{
                        textAlign: 'center',
                        padding: '0px 0px 0px 0px',
                        fontSize: '10px',
                      }}
                      className="Professionaluseonly"
                    >
                      PROFESSIONAL USE ONLY
                    </div>
                  </div>
                </div>
              </div>
              <div style={{ paddingLeft: '40px' }}>
                <div
                  style={{
                    // background: `url('http://createyourformula.hautecustombeauty.com/assets/img/pdf/bg_color_image_square.png')`,
                    backgroundRepeat: 'no-repeat',
                    backgroundSize: 'contain',
                    width: '100mm',
                    height: '130mm',
                    // border: '2px solid #f46ebd',
                  }}
                >
                  {/* <div
                    style={{
                      display: 'flex',
                    }}
                  >
                    <img
                      src="http://createyourformula.hautecustombeauty.com/assets/img/pdf/MD-Logo.png" // Ensure the protocol (http:// or https://)
                      alt="PDF Logo" // A more descriptive alt text
                      style={{
                        height: '45px',
                        width: '43%',
                        marginTop: '12px',
                      }}
                    />
                    <img
                      src="http://createyourformula.hautecustombeauty.com/assets/img/pdf/H-Logos.png" // Ensure the protocol (http:// or https://)
                      alt="PDF Logo" // A more descriptive alt text
                      style={{
                        height: '55px',
                        width: '57%',
                        marginTop: '5px',
                        marginRight: '5px',
                      }}
                    />
                  </div> */}
                  <div
                    style={{
                      // width: '100%',
                      // height: '100%',
                      padding: '80px 30px',
                    }}
                  >
                    <div
                      style={{
                        fontSize: '15px',
                        padding: '7px',
                      }}
                      className="CANISTERNAME"
                    />
                    <div
                      style={{
                        fontSize: '15px',
                        padding: '7px',
                      }}
                      className="CANISTERCODE"
                    />
                    <div
                      style={{
                        fontSize: '15px',
                        padding: '7px 7px 40px 7px',
                      }}
                      className="CANISTERSKU"
                    />

                    <div
                      style={{
                        height: '50mm',
                        width: '80mm',
                        textAlign: 'center',
                      }}
                      id="QRCODE"
                    >
                      <img className="QRCODE" alt="" />
                    </div>
                  </div>
                  {/* <div
                    style={{
                      display: 'flex',
                    }}
                  >
                    <img
                      src="http://createyourformula.hautecustombeauty.com/assets/img/pdf/H-Logos.png" // Ensure the protocol (http:// or https://)
                      alt="PDF Logo" // A more descriptive alt text
                      style={{
                        height: '55px',
                        width: '57%',
                        marginTop: '5px',
                        margin: '5px 5px 0px 5px',
                      }}
                    />
                    <div style={{ padding: '0px 12px 0px 5px' }}>
                      <img
                        src="http://createyourformula.hautecustombeauty.com/assets/img/pdf/pdf_logo.png" // Ensure the protocol (http:// or https://)
                        alt="PDF Logo" // A more descriptive alt text
                        style={{
                          height: '45%',
                          width: '100%',
                          marginTop: '7px',
                        }}
                      />
                      <p
                        style={{
                          fontSize: '10px',
                          margin: '5px 0px 0px 0px',
                          textAlign: 'center',
                          color: '#988a6d',
                        }}
                      >
                        AUSTIN . BARCELONA
                      </p>
                    </div>
                  </div> */}
                </div>
              </div>

              {/* </div> */}
            </section>
          </div>
        </div>
      </div>
    </>
  );
};
export default PdfDocument;
