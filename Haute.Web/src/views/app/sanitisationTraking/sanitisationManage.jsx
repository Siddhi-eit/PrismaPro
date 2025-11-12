import React, { useState, useEffect } from 'react';
import useMousetrap from 'hooks/use-mousetrap';
import { connect } from 'react-redux';
import Grid from 'containers/sanitisationTraking/Grid';
import PageHeader from 'containers/pages/sanitisationTraking/PageHeader';
// import UserModal from 'containers/pages/userManagement/UserModal';
// import { ResetState } from 'redux/actions';
// import {SanitisationTrakingByID} from 'redux/actions';
import {
  addSanitisationTraking,
  deleteSanitisationTraking,
  getSanitisationList,
  ResetState,
  SanitisationTrakingByID,
  bindMDFusionLabNoDropdown,
  bindCanisterNoDropdown,
  bindProductDropdown,
  downloadSanitisationExcelFile,
} from 'redux/actions';
import createNotification from 'helpers/alerts';
import SanitisationTrakingModal from 'containers/pages/sanitisationTraking/SanitisationTrakingModal';
// import PdfDocument from 'helpers/PdfDocument';
import {
  SUCCESSFULLY_ADDED,
  getCurrentUser,
  getMachineID,
} from 'helpers/Utils';
import DeleteModal from 'containers/pages/DeleteModal';
import SanitisationPDFDocument from 'helpers/SanitisationPDFDocument';
// import PdfDocument from 'helpers/PdfDocument';

const getIndex = (value, arr, prop) => {
  for (let i = 0; i < arr.length; i += 1) {
    if (arr[i][prop] === value) {
      return i;
    }
  }
  return -1;
};

const reminderData = [
  { title: '06', value: 6 },
  { title: '08', value: 8 },
  { title: '10', value: 10 },
];
const reminderOptionGroups = {
  reminder: reminderData.map((i) => ({ value: i.value, label: i.title })),
};
const stickerData = [
  { title: '01', value: 1 },
  { title: '02', value: 2 },
  { title: '03', value: 3 },
  { title: '04', value: 4 },
  { title: '05', value: 5 },
];
const stickerOptionGroups = {
  sticker: stickerData.map((i) => ({ value: i.value, label: i.title })),
};
const orderOptions = [
  { column: '3', label: 'FusionLab No' },
  { column: '4', label: 'Canister No' },
  { column: '5', label: 'Sanitised Date' },
  { column: '8', label: 'Created Date' },
  { column: '19', label: 'Sanitized?' },
  { column: '10', label: 'Is Active ?' },
];
const orderDirection = [
  { sortOrder: 'asc', label: 'A-Z' },
  { sortOrder: 'desc', label: 'Z-A' },
];

const pageSizes = [10, 20, 50, 100];
const dateSanitizedList = [
  { label: '10 Month', value: '10', key: 0 },
  { label: '8 Month', value: '8', key: 1 },
  { label: '6 Month', value: '6', key: 2 },
];

const SanitisationList = ({
  match,
  SanitisationListAction,
  allSanitisationItems,
  isSucessfullyAdd,
  isSucessfullyDelete,
  resultMessage,
  loading,
  bindCanisterNoDropdownAction,
  canisterNoDropdownList,
  machineID,
  AddSanitisationTrakingAction,
  ResetStateAction,
  sanitisationDetail,
  getByIDAction,
  deleteSanitisationTrakingAction,
  sanitisationPDFData,
  bindMDFusionLabNoDropdownAction,
  mdFusionLabNoDropdownList,
  bindProductDropdownAction,
  productDropdownList,
  DownloadSanitisationExcelFileAction,
}) => {
  // const [mdFusionLabNodropdownItem, setMdFusionLabNodropdownItem] = useState([]);
  const [currentUser, setCurrentUser] = useState(0);
  const [valueForQRCode, setValueForQRCode] = useState(null);
  const [valueForPDFContent, SetValueForPDFContent] = useState(null);
  const [isLoaded, setIsLoaded] = useState(false);
  const [displayMode, setDisplayMode] = useState('thumblist');
  const [currentPage, setCurrentPage] = useState(1);
  const [selectedPageSize, setSelectedPageSize] = useState(10);
  const [selectedOrderOption, setSelectedOrderOption] = useState({
    column: '8',
    label: 'Created Date',
  });
  const [modalOpen, setModalOpen] = useState(false);
  const [totalItemCount, setTotalItemCount] = useState(0);
  const [totalPage, setTotalPage] = useState(1);
  const [search, setSearch] = useState('');
  const [selectedItems, setSelectedItems] = useState([]);
  const [selectedUserID, setSelectedUserID] = useState('');
  const [items, setItems] = useState([]);
  const [lastChecked, setLastChecked] = useState(null);
  const [isPdf, setIsPdf] = useState(false);
  const [deleteMessage, setDeleteMessage] = useState(null);
  const [isDeleteModalOpen, seIsDeleteModalOpen] = useState(false);
  // const [btnLoader, setbtnLoader] = useState(false);
  const [mdFusionLabNodropdownItem, setMdFusionLabNodropdownItem] = useState(
    []
  );
  const [canisterNodropdownItem, setCanisterNodropdownItem] = useState([]);
  const [productdropdownItem, setProductdropdownItem] = useState([]);
  const [productItems, setProductItems] = useState([]);
  const [selectedOrderDirection, setSelectedOrderDirection] = useState({
    sortOrder: 'desc',
    label: 'Z-A',
  });

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
        machineID: machineID,
      };
      SanitisationListAction(gridObject);
    }
    fetchData();
  }, [
    selectedPageSize,
    currentPage,
    selectedOrderOption,
    search,
    SanitisationListAction,
    machineID,
    selectedOrderDirection,
  ]);
  useEffect(() => {
    if (isSucessfullyAdd > 0) {
      setModalOpen(!modalOpen);
      ResetStateAction();
    }
    if (allSanitisationItems != null) {
      setTotalPage(allSanitisationItems.totalPage);
      setItems(
        allSanitisationItems.data.map((x) => {
          return { ...x };
        })
      );
      setSelectedItems([]);
      setTotalItemCount(allSanitisationItems.totalItem);
      setIsLoaded(true);
    }
  }, [
    allSanitisationItems,
    selectedPageSize,
    currentPage,
    selectedOrderOption,
    selectedOrderDirection,
    search,
    isSucessfullyAdd,
    modalOpen,
    loading,
  ]);

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
        orderDirection: selectedOrderDirection.sortOrder,
        search,
      };
      SanitisationListAction(gridObject);
    }
  }, [
    isSucessfullyDelete,
    isSucessfullyAdd,
    selectedPageSize,
    currentPage,
    selectedOrderOption.column,
    selectedOrderDirection.sortOrder,
    search,
    SanitisationListAction,
    machineID,
  ]);

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
        machineID: machineID,
      };
      SanitisationListAction(gridObject);
    }
    if (isSucessfullyAdd != null && isSucessfullyAdd > 0) {
      //  const dataaa=  PdfDocument();
      setValueForQRCode(isSucessfullyAdd);
      if (SUCCESSFULLY_ADDED === resultMessage) {
        setIsPdf(true);
        SetValueForPDFContent(sanitisationPDFData);
      }
      createNotification('success', 'Success', resultMessage);
    }
    // if (SUCCESSFULLY_ADDED === resultMessage && isSucessfullyAdd != null &&  isSucessfullyAdd > 0) {
    //   setIsPdf(true);
    // }
    if (isSucessfullyAdd != null && isSucessfullyAdd < 0) {
      createNotification('warning', 'Warning', resultMessage);
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
  }, [isSucessfullyDelete, isSucessfullyAdd, resultMessage, isPdf]);

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
  useEffect(() => {
    if (sanitisationDetail != null) {
      setModalOpen(!modalOpen);
    }
  }, [sanitisationDetail]);
  const onContextMenuClick = (e, data) => {
    if (data.action === 'delete') {
      setDeleteMessage('Are you sure you want to delete?');
      seIsDeleteModalOpen(!isDeleteModalOpen);
      ResetStateAction();
      setIsPdf(false);
    } else if (data.action === 'Edit') {
      setIsPdf(false);
      bindCanisterNoDropdownAction(machineID);
      getByIDAction(selectedUserID);
    }
  };
  const onContextMenu = (e, data) => {
    const clickedProductId = data.data;
    setSelectedUserID(clickedProductId);
    if (!selectedItems.includes(clickedProductId)) {
      setSelectedItems([clickedProductId]);
    }
    return true;
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
    bindCanisterNoDropdownAction(machineID);
  }, [machineID]);
  useEffect(() => {
    if (canisterNoDropdownList != null && canisterNoDropdownList.length > 0) {
      const canisterNoOption = canisterNoDropdownList.map((d) => ({
        value: d.id,
        label: d.canisterCode,
      }));
      const productOption = canisterNoDropdownList.map((d) => ({
        value: d.id,
        label: d.name,
      }));
      setProductdropdownItem(productOption);
      setCanisterNodropdownItem(canisterNoOption);
    }
  }, [canisterNoDropdownList]);

  const onSanitisationSubmit = (values) => {
    let machineID = getMachineID();
    let user = getCurrentUser();
    let value = {
      id: values.id,
      machineID: machineID,
      userID: user.uid,
      fusionLabNo: values.fusionLabNo,
      canisterID: values.canisterID,
      isActive: values.isActive,
    };
    AddSanitisationTrakingAction(value);
  };
  const onDeleteClick = () => {
    seIsDeleteModalOpen(!isDeleteModalOpen);
    deleteSanitisationTrakingAction(selectedUserID);
  };
  useEffect(() => {
    document.getElementById('machineDropdown').style.display = 'block';
  }, []);

  const ExportSanitisationData = () => {
    DownloadSanitisationExcelFileAction();
  };

  return (
    <>
      {loading && (
        <div className="customOverlay">
          <div className="loading" />
        </div>
      )}
      {isPdf && valueForQRCode !== null ? (
        <SanitisationPDFDocument
          valueForQRCode={valueForQRCode}
          pageName="SanitisationTracking"
          valueForPDFContent={valueForPDFContent}
        />
      ) : null}
      <div className="disable-text-selection">
        <PageHeader
          heading="Sanitisation"
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
            ResetStateAction();
            // UserAddUserResetStateAction();
          }}
          orderDirection={orderDirection}
          selectedOrderDirection={selectedOrderDirection}
          changeOrderDirection={(sortOrder) => {
            setSelectedOrderDirection(
              orderDirection.find((x) => x.sortOrder === sortOrder)
            );
          }}
          ExportData={() => {
            ExportSanitisationData();
          }}
        />
        <SanitisationTrakingModal
          modalOpen={modalOpen}
          toggleModal={() => {
            setModalOpen(!modalOpen);
            ResetStateAction();
          }}
          mdFusionLabNoList={mdFusionLabNodropdownItem}
          canisterNOList={canisterNodropdownItem}
          dateSanitizedList={dateSanitizedList}
          productList={productdropdownItem}
          onSanitisationSubmit={onSanitisationSubmit}
          sanitisationDetail={sanitisationDetail}
        />
        <Grid
          items={items}
          allSanitisationItems={allSanitisationItems}
          displayMode={displayMode}
          selectedItems={selectedItems}
          onCheckItem={onCheckItem}
          currentPage={currentPage}
          totalPage={totalPage}
          onContextMenuClick={onContextMenuClick}
          onContextMenu={onContextMenu}
          onChangePage={setCurrentPage}
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

const mapStateToProps = ({ sanitisation, user }) => {
  const {
    allSanitisationItems,
    isSucessfullyAdd,
    isSucessfullyDelete,
    resultMessage,
    loading,
    canisterNoDropdownList,
    sanitisationDetail,
    sanitisationPDFData,
  } = sanitisation;
  const { machineID, mdFusionLabNoDropdownList } = user;
  return {
    allSanitisationItems,
    isSucessfullyAdd,
    isSucessfullyDelete,
    resultMessage,
    loading,
    canisterNoDropdownList,
    machineID,
    sanitisationDetail,
    sanitisationPDFData,
    mdFusionLabNoDropdownList,
  };
};
export default connect(mapStateToProps, {
  SanitisationListAction: getSanitisationList,
  bindCanisterNoDropdownAction: bindCanisterNoDropdown,
  ResetStateAction: ResetState,
  AddSanitisationTrakingAction: addSanitisationTraking,
  getByIDAction: SanitisationTrakingByID,
  deleteSanitisationTrakingAction: deleteSanitisationTraking,
  bindMDFusionLabNoDropdownAction: bindMDFusionLabNoDropdown,
  DownloadSanitisationExcelFileAction: downloadSanitisationExcelFile,
})(SanitisationList);
