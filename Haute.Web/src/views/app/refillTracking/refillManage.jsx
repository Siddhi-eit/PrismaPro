import React, { useState, useEffect } from 'react';
import useMousetrap from 'hooks/use-mousetrap';
import { connect } from 'react-redux';
import Grid from 'containers/refllManagement/Grid';
import PageHeader from 'containers/pages/refillManagement/PageHeader';
import {
  bindCanisterNoDropdown,
  getRefillList,
  addRefillTraking,
  downloadRefillScanner,
  bindDispenseUnitDropdown,
  ResetStateRefillTrking,
  RefillTrkingGetByID,
  deleteRefillTracking,
  bindMDFusionLabNoDropdown,
  bindProductDropdown,
  downloadRefillExcelFile,
  clearRefillScanner,
} from 'redux/actions';
import RefillTrakingModal from 'containers/pages/refillManagement/RefillTrakingModal';
import createNotification from 'helpers/alerts';
import PdfDocument from 'helpers/PdfDocument';
import {
  SUCCESSFULLY_ADDED,
  getCurrentUser,
  getMachineID,
} from 'helpers/Utils';
import DeleteModal from 'containers/pages/DeleteModal';

const getIndex = (value, arr, prop) => {
  for (let i = 0; i < arr.length; i += 1) {
    if (arr[i][prop] === value) {
      return i;
    }
  }
  return -1;
};

const reminderData = [
  { title: '250', value: 250 },
  { title: '500', value: 500 },
  { title: '750', value: 750 },
  { title: '1,000', value: 1000 },
  { title: '1,500', value: 1500 },
  { title: '2,000 ', value: 2000 },
];
const reminderOptionGroups = {
  reminder: reminderData.map((i) => ({ value: i.value, label: i.title })),
};

const orderOptions = [
  { column: '13', label: 'Canister Code' },
  { column: '18', label: 'Fusion Lab No' },
  { column: '3', label: 'DateFilled' },
  { column: '18', label: 'Lot Nr.' },
  { column: '4', label: 'Quantity' },
  { column: '6', label: 'Created Date' },
  { column: '17', label: 'Refilled?' },
  { column: '19', label: 'Is Active ?' },
];
const orderDirection = [
  { sortOrder: 'asc', label: 'A-Z' },
  { sortOrder: 'desc', label: 'Z-A' },
];
const pageSizes = [10, 20, 50, 100];
// const dateFilledList = [
//   { label: '10 Month', value: '10', key: 0 },
//   { label: '8 Month', value: '8', key: 1 },
//   { label: '6 Month', value: '6', key: 2 },
// ];

const RefillList = ({
  match,
  RefillListAction,
  allRefillItems,
  loading,
  canisterNoDropdownList,
  mdFusionLabNoDropdownList,
  AddRefillTrakingAction,
  machineID,
  bindCanisterNoDropdownAction,
  BindDispenseUnitDropdown,
  dispenseUnitDropdownList,
  isSucessfullyAdd,
  resultMessage,
  isSucessfullyDelete,
  ResetStateRefillTrkingAction,
  RefillTrkingGetByIDAction,
  refillDetail,
  deleteRefillTrackingAction,
  refillPDFData,
  refillScannerData,
  bindMDFusionLabNoDropdownAction,
  bindProductDropdownAction,
  productDropdownList,
  DownloadRefillExcelFileAction,
  OnDownloadScanner,
  onClearRefillScanner,
}) => {
  const [currentUser, setCurrentUser] = useState(0);
  const [isPdf, setIsPdf] = useState(false);
  const [valueForQRCode, setValueForQRCode] = useState(null);
  const [valueForPDFContent, SetValueForPDFContent] = useState(null);
  const [isDeleteModalOpen, seIsDeleteModalOpen] = useState(false);
  const [deleteMessage, setDeleteMessage] = useState(null);
  const [displayMode, setDisplayMode] = useState('thumblist');
  const [currentPage, setCurrentPage] = useState(1);
  const [selectedPageSize, setSelectedPageSize] = useState(10);
  const [selectedOrderOption, setSelectedOrderOption] = useState({
    column: '6',
    label: 'Created Date',
  });
  const [modalOpen, setModalOpen] = useState(false);
  const [totalItemCount, setTotalItemCount] = useState(0);
  const [totalPage, setTotalPage] = useState(1);
  const [search, setSearch] = useState('');
  const [selectedItems, setSelectedItems] = useState([]);
  const [items, setItems] = useState([]);
  const [lastChecked, setLastChecked] = useState(null);
  const [canisterNodropdownItem, setCanisterNodropdownItem] = useState([]);
  const [mdFusionLabNodropdownItem, setMdFusionLabNodropdownItem] = useState(
    []
  );
  const [productdropdownItem, setProductdropdownItem] = useState([]);
  const [selectedID, setSelectedID] = useState('');
  const [productItems, setProductItems] = useState([]);
  const [selectedOrderDirection, setSelectedOrderDirection] = useState({
    sortOrder: 'desc',
    label: 'Z-A',
  });
  useEffect(() => {
    setIsPdf(false);
    onClearRefillScanner();
  }, []);

  useEffect(() => {
    setCurrentPage(1);
  }, [selectedPageSize, selectedOrderOption]);
  useEffect(() => {
    async function fetchData() {
      const gridObject = {
        pageSize: selectedPageSize,
        currentPage,
        orderBy: selectedOrderOption.column,
        search,
        orderDirection: selectedOrderDirection.sortOrder,
        userID: machineID,
      };
      RefillListAction(gridObject);
    }
    fetchData();
  }, [
    selectedPageSize,
    currentPage,
    selectedOrderOption,
    selectedOrderDirection,
    search,
    machineID,
  ]);
  useEffect(() => {
    setIsPdf(false);
    if (isSucessfullyAdd != null && isSucessfullyAdd > 0) {
      setModalOpen(!modalOpen);
      ResetStateRefillTrkingAction();
    }
    if (allRefillItems != null) {
      setTotalPage(allRefillItems.totalPage);
      setItems(
        allRefillItems.data.map((x) => {
          return { ...x };
        })
      );
      setSelectedItems([]);
      setTotalItemCount(allRefillItems.totalItem);
    }
  }, [
    allRefillItems,
    selectedPageSize,
    currentPage,
    selectedOrderOption,
    selectedOrderDirection,
    search,
    isSucessfullyAdd,
  ]);

  const onCheckItem = (event, id) => {
    if (
      event.target.tagName === 'A' ||
      (event.target.parentElement && event.target.parentElement.tagName === 'A')
    ) {
      return true;
    }
    if (lastChecked === null) {
      setLastChecked(id);
    }
    let selectedList = [...selectedItems];
    if (selectedList.includes(id)) {
      selectedList = selectedList.filter((x) => x !== id);
    } else {
      selectedList.push(id);
    }
    setSelectedItems(selectedList);
    if (event.shiftKey) {
      let newItems = [...items];
      const start = getIndex(id, newItems, 'id');
      const end = getIndex(lastChecked, newItems, 'id');
      newItems = newItems.slice(Math.min(start, end), Math.max(start, end) + 1);
      selectedItems.push(
        ...newItems.map((item) => {
          return item.id;
        })
      );
      selectedList = Array.from(new Set(selectedItems));
      setSelectedItems(selectedList);
    }
    document.activeElement.blur();
    return false;
  };
  const handleChangeSelectAll = (isToggle) => {
    if (selectedItems.length >= items.length) {
      if (isToggle) {
        setSelectedItems([]);
      }
    } else {
      setSelectedItems(items.map((x) => x.id));
    }
    document.activeElement.blur();
    return false;
  };

  useMousetrap(['ctrl+a', 'command+a'], () => {
    handleChangeSelectAll(false);
  });
  useMousetrap(['ctrl+d', 'command+d'], () => {
    setSelectedItems([]);
    return false;
  });
  const startIndex = (currentPage - 1) * selectedPageSize;
  const endIndex = currentPage * selectedPageSize;

  useEffect(() => {
    if (canisterNoDropdownList != null && canisterNoDropdownList.length > 0) {
      const canisterNoOption = canisterNoDropdownList.map((d) => ({
        value: d.id,
        label: d.canisterCode,
      }));

      setCanisterNodropdownItem(canisterNoOption);
    }
  }, [canisterNoDropdownList]);

  useEffect(() => {
    bindMDFusionLabNoDropdownAction(machineID);
  }, [bindMDFusionLabNoDropdownAction]);
  useEffect(
    () => {
      if (
        mdFusionLabNoDropdownList != null &&
        mdFusionLabNoDropdownList.length > 0
      ) {
        const mdFusionLabNoOption = mdFusionLabNoDropdownList.map((d) => ({
          value: d.value,
          label: d.label,
        }));
        setMdFusionLabNodropdownItem(mdFusionLabNoOption);
      }
    },
    [mdFusionLabNoDropdownList],
    [canisterNoDropdownList]
  );

  useEffect(() => {
    bindProductDropdownAction();
  }, [bindProductDropdownAction]);
  useEffect(() => {
    if (productDropdownList != null && productDropdownList.length > 0) {
      const productOption = productDropdownList.map((d) => ({
        value: d.value,
        label: d.label.trim(),
      }));
      setProductItems(productOption);
    }
  }, [productDropdownList]);

  const onRefillSubmit = (values) => {
    let machineID = getMachineID();
    let user = getCurrentUser();
    let value = {
      id: values.id,
      machineID: machineID,
      userID: user.uid,
      fusionLabNo: values.fusionLabNo,
      canisterNO: values.canisterNO,
      refillML: values.refillCanSize,
      // product: values.product,
      isActive: values.isActive,
      // refillCanSize: refillCanSize
    };
    AddRefillTrakingAction(value);
  };

  const onContextMenuClick = (e, data) => {
    if (data.action === 'delete') {
      setDeleteMessage('Are you sure you want to delete?');
      seIsDeleteModalOpen(!isDeleteModalOpen);
      ResetStateRefillTrkingAction();
      setIsPdf(false);
    } else if (data.action === 'Edit') {
      setIsPdf(false);
      bindCanisterNoDropdownAction(machineID);
      RefillTrkingGetByIDAction(selectedID);
    }
  };

  const onContextMenu = (e, data) => {
    const clickedProductId = data.data;
    setSelectedID(clickedProductId);
    if (!selectedItems.includes(clickedProductId)) {
      setSelectedItems([clickedProductId]);
    }
    return true;
  };

  useEffect(() => {
    bindCanisterNoDropdownAction(machineID);
  }, [machineID]);
  useEffect(() => {
    BindDispenseUnitDropdown();
    document.getElementById('machineDropdown').style.display = 'block';
  }, []);
  const [unitTypedropdownItem, setUnitTypedropdownItem] = useState([]);
  useEffect(() => {
    if (
      dispenseUnitDropdownList != null &&
      dispenseUnitDropdownList.length > 0
    ) {
      const options = dispenseUnitDropdownList.map((d) => ({
        value: d.id,
        label: d.name,
      }));
      setUnitTypedropdownItem(options);
    }
  }, [dispenseUnitDropdownList]);

  useEffect(() => {
    if (
      (isSucessfullyDelete !== null &&
        isSucessfullyDelete !== undefined &&
        isSucessfullyDelete !== 0) ||
      (isSucessfullyAdd != null && isSucessfullyAdd > 0)
    ) {
      const gridObject = {
        pageSize: selectedPageSize,
        currentPage,
        orderBy: selectedOrderOption.column,
        search,
        orderDirection: selectedOrderDirection.sortOrder,
        userID: machineID,
      };
      RefillListAction(gridObject);
    }
    if (isSucessfullyAdd != null && isSucessfullyAdd > 0) {
      if (SUCCESSFULLY_ADDED === resultMessage) {
        setValueForQRCode(isSucessfullyAdd);
        SetValueForPDFContent(refillPDFData);
        setIsPdf(true);
      }
      createNotification('success', 'Success', resultMessage);
    }
    if (refillScannerData != null) {
      setValueForQRCode(refillScannerData.id);
      SetValueForPDFContent(refillScannerData);
      setIsPdf(true);
    }
    if (isSucessfullyAdd != null && isSucessfullyAdd === 0) {
      createNotification('error', 'Error', resultMessage);
    }
    if (
      (isSucessfullyDelete !== null || isSucessfullyDelete !== undefined) &&
      isSucessfullyDelete !== 0
    ) {
      createNotification('success', 'Success', 'Deleted Successfully !!');
    }
  }, [isSucessfullyDelete, isSucessfullyAdd, refillScannerData]);
  useEffect(() => {
    if (refillDetail != null) {
      setModalOpen(!modalOpen);
    }
  }, [refillDetail]);
  const onDeleteClick = () => {
    seIsDeleteModalOpen(!isDeleteModalOpen);
    deleteRefillTrackingAction(selectedID);
  };

  const ExportRefillData = () => {
    DownloadRefillExcelFileAction();
  };

  const DownloadScanner = (id) => {
    console.log('id', id);
    OnDownloadScanner(id);
  };

  const ClearRefillScanner = () => {
    onClearRefillScanner();
  };

  return (
    <>
      {loading && (
        <div className="customOverlay">
          <div className="loading" />
        </div>
      )}
      {isPdf && valueForQRCode !== null ? (
        <PdfDocument
          valueForQRCode={valueForQRCode}
          pageName="RefillTracking"
          valueForPDFContent={valueForPDFContent}
          clearRefillScanner={ClearRefillScanner}
        />
      ) : null}
      <div className="disable-text-selection">
        <PageHeader
          heading="Refill"
          displayMode={displayMode}
          changeDisplayMode={setDisplayMode}
          handleChangeSelectAll={handleChangeSelectAll}
          changeOrderBy={(column) => {
            setSelectedOrderOption(
              orderOptions.find((x) => x.column === column)
            );
          }}
          changePageSize={setSelectedPageSize}
          selectedPageSize={selectedPageSize}
          totalItemCount={totalItemCount}
          selectedOrderOption={selectedOrderOption}
          match={match}
          startIndex={startIndex}
          endIndex={endIndex}
          selectedItemsLength={selectedItems ? selectedItems.length : 0}
          itemsLength={items ? items.length : 0}
          onSearchKey={(e) => {
            if (e.key === 'Enter') {
              setSearch(e.target.value.toLowerCase());
            }
          }}
          orderOptions={orderOptions}
          pageSizes={pageSizes}
          toggleModal={() => {
            setModalOpen(!modalOpen);
            ResetStateRefillTrkingAction();
          }}
          orderDirection={orderDirection}
          selectedOrderDirection={selectedOrderDirection}
          changeOrderDirection={(sortOrder) => {
            setSelectedOrderDirection(
              orderDirection.find((x) => x.sortOrder === sortOrder)
            );
          }}
          ExportData={() => {
            ExportRefillData();
          }}
        />
        <RefillTrakingModal
          modalOpen={modalOpen}
          toggleModal={() => {
            setModalOpen(!modalOpen);
            ResetStateRefillTrkingAction();
          }}
          mdFusionLabNoList={mdFusionLabNodropdownItem}
          canisterNOList={canisterNodropdownItem}
          refillCanSizeitem={reminderOptionGroups}
          productList={productItems}
          unitTypedropdownItem={unitTypedropdownItem}
          refillDetail={refillDetail}
          onRefillSubmit={onRefillSubmit}
        />
        <Grid
          items={items}
          allRefillItems={allRefillItems}
          displayMode={displayMode}
          selectedItems={selectedItems}
          onCheckItem={onCheckItem}
          currentPage={currentPage}
          totalPage={totalPage}
          onContextMenuClick={onContextMenuClick}
          onContextMenu={onContextMenu}
          onChangePage={setCurrentPage}
          onDownload={DownloadScanner}
        />
        <DeleteModal
          deleteMessage={deleteMessage}
          isDeleteModalOpen={isDeleteModalOpen}
          deletedClick={() => onDeleteClick()}
          toggleModal={() => seIsDeleteModalOpen(!isDeleteModalOpen)}
        />
      </div>
    </>
  );
};

const mapStateToProps = ({ refill, sanitisation, user, canister }) => {
  const {
    allRefillItems,
    isSucessfullyAdd,
    isAddError,
    loading,
    resultMessage,
    isSucessfullyDelete,
    refillDetail,
    refillPDFData,
    refillScannerData,
  } = refill;
  const { machineID, mdFusionLabNoDropdownList } = user;
  const { canisterNoDropdownList } = sanitisation;
  const { dispenseUnitDropdownList, productDropdownList } = canister;
  return {
    allRefillItems,
    isSucessfullyAdd,
    isAddError,
    loading,
    canisterNoDropdownList,
    machineID,
    dispenseUnitDropdownList,
    resultMessage,
    isSucessfullyDelete,
    refillDetail,
    refillPDFData,
    mdFusionLabNoDropdownList,
    productDropdownList,
    refillScannerData,
  };
};
export default connect(mapStateToProps, {
  RefillListAction: getRefillList,
  AddRefillTrakingAction: addRefillTraking,
  OnDownloadScanner: downloadRefillScanner,
  bindCanisterNoDropdownAction: bindCanisterNoDropdown,
  BindDispenseUnitDropdown: bindDispenseUnitDropdown,
  ResetStateRefillTrkingAction: ResetStateRefillTrking,
  RefillTrkingGetByIDAction: RefillTrkingGetByID,
  deleteRefillTrackingAction: deleteRefillTracking,
  bindMDFusionLabNoDropdownAction: bindMDFusionLabNoDropdown,
  bindProductDropdownAction: bindProductDropdown,
  DownloadRefillExcelFileAction: downloadRefillExcelFile,
  onClearRefillScanner: clearRefillScanner,
})(RefillList);
