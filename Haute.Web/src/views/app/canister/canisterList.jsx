import React, { useState, useEffect } from 'react';
// import ListPageHeading from 'containers/pages/ListPageHeading';
// import ListPageListing from 'containers/pages/ListPageListing';
// import AddNewModal from 'containers/pages/AddNewModal';
import useMousetrap from 'hooks/use-mousetrap';
import { getCurrentUser } from 'helpers/Utils';
import { connect } from 'react-redux';
import CanisterListing from 'containers/canisterGrid/CanisterListing';
import CanisterListPageHeading from 'containers/pages/CanisterListPageHeading';
import CanisterModal from 'containers/pages/CanisterModal';
import DeleteModal from 'containers/pages/DeleteModal';
import {
  addCanister,
  addCanisterResetState,
  deleteCanister,
  getCanisterList,
  getByID,
  bindDispenseUnitDropdown,
  downloadCanisterExcelFile,
  getCanisterLookupByCanisterCode,
} from 'redux/actions';
import createNotification from 'helpers/alerts';
// import { getMachineID } from 'helpers/Utils';

const getIndex = (value, arr, prop) => {
  for (let i = 0; i < arr.length; i += 1) {
    if (arr[i][prop] === value) {
      return i;
    }
  }
  return -1;
};
const orderOptions = [
  { column: '3', label: 'Canister Code' },
  { column: '4', label: 'SKU' },
  { column: '9', label: 'Name' },
  { column: '17', label: 'CurrentAmount' },
  { column: '2', label: 'MaximumAmount' },
  { column: '16', label: 'MinimumAmount' },
  { column: '18', label: 'WarningAmount' },
  { column: '6', label: 'Created Date' },
];
const orderDirection = [
  { sortOrder: 'asc', label: 'A-Z' },
  { sortOrder: 'desc', label: 'Z-A' },
];

const pageSizes = [10, 20, 50, 100];
const CanisterList = ({
  match,
  CanisterListAction,
  allCanisterItems,
  CanisterDeleteAction,
  isSucessfullyDelete,
  CanisterAddAction,
  isSucessfullyAdd,
  CanisterAddCanisterResetStateAction,
  CanisterGetByIDAction,
  canisterDetail,
  loading,
  machineID,
  lookupData,
  BindDispenseUnitDropdown,
  dispenseUnitDropdownList,
  resultMessage,
  checkIsCanister,
  DownloadCanisterExcelFileAction,
  OnGetCanisterLookup,
}) => {
  const [isLoaded, setIsLoaded] = useState(false);
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
  const [selectedCanisterID, setSelectedCanisterID] = useState('');
  const [items, setItems] = useState([]);
  const [lastChecked, setLastChecked] = useState(null);
  const [deleteMessage, setDeleteMessage] = useState(null);
  const [isDeleteModalOpen, seIsDeleteModalOpen] = useState(false);
  const [canisterLookupData, setCanisterLookupData] = useState([]);
  const [selectedOrderDirection, setSelectedOrderDirection] = useState({
    sortOrder: 'desc',
    label: 'Z-A',
  });

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
      CanisterListAction(gridObject);
    }
    fetchData();
  }, [
    selectedPageSize,
    currentPage,
    selectedOrderOption,
    search,
    machineID,
    selectedOrderDirection,
  ]);
  useEffect(() => {
    if (isSucessfullyAdd != null && isSucessfullyAdd > 0) {
      setModalOpen(!modalOpen);
      CanisterAddCanisterResetStateAction();
    }
    if (allCanisterItems != null) {
      setTotalPage(allCanisterItems.totalPage);
      setItems(
        allCanisterItems.data.map((x) => {
          return { ...x };
        })
      );
      setSelectedItems([]);
      setTotalItemCount(allCanisterItems.totalItem);
      setIsLoaded(true);
    }
  }, [
    allCanisterItems,
    selectedPageSize,
    currentPage,
    selectedOrderOption,
    search,
    isSucessfullyAdd,
    selectedOrderDirection,
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
      CanisterListAction(gridObject);
    }
    if (isSucessfullyAdd != null && isSucessfullyAdd > 0) {
      createNotification('success', 'Success', resultMessage);
    }
    if (isSucessfullyAdd != null && isSucessfullyAdd === -1) {
      createNotification('warning', 'Warning', resultMessage);
    }
    if (isSucessfullyAdd != null && isSucessfullyAdd === -2) {
      createNotification('warning', 'Warning', resultMessage);
    }
    if (
      (isSucessfullyDelete !== null || isSucessfullyDelete !== undefined) &&
      isSucessfullyDelete !== 0
    ) {
      createNotification('success', 'Success', 'Deleted Successfully !!');
    }
  }, [isSucessfullyDelete, isSucessfullyAdd]);

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
  const onContextMenuClick = (e, data) => {
    if (data.action === 'delete') {
      setDeleteMessage('Are you sure you want to delete?');
      seIsDeleteModalOpen(!isDeleteModalOpen);
      CanisterAddCanisterResetStateAction();
    } else if (data.action === 'Edit') {
      CanisterGetByIDAction(selectedCanisterID);
    }
  };
  useEffect(() => {
    if (canisterDetail != null) {
      OnGetCanisterLookup();
    }
  }, [canisterDetail]);
  const onContextMenu = (e, data) => {
    const clickedProductId = data.data;
    setSelectedCanisterID(clickedProductId);
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

  const onDeleteClick = () => {
    seIsDeleteModalOpen(!isDeleteModalOpen);
    CanisterDeleteAction(selectedCanisterID);
  };
  const onCanisterSubmit = (value) => {
    CanisterAddAction(value);
  };

  useEffect(() => {
    BindDispenseUnitDropdown();
  }, []);
  const [machineDropdownItem, setmachineDropdownItem] = useState([]);
  useEffect(() => {
    if (
      dispenseUnitDropdownList != null &&
      dispenseUnitDropdownList.length > 0
    ) {
      const options = dispenseUnitDropdownList.map((d) => ({
        value: d.id,
        label: d.name,
      }));
      setmachineDropdownItem(options);
    }
  }, [dispenseUnitDropdownList]);

  useEffect(() => {
    document.getElementById('machineDropdown').style.display = 'block';
  }, []);

  const ExportCanisterData = () => {
    DownloadCanisterExcelFileAction();
  };

  useEffect(() => {
    console.log('lookupData', lookupData);
    if (lookupData && lookupData.length > 0) {
      setModalOpen(!modalOpen);
      setCanisterLookupData(lookupData);
    }
  }, [lookupData]);

  return (
    <>
      {loading && (
        <div className="customOverlay">
          <div className="loading" />
        </div>
      )}
      <div className="disable-text-selection">
        <CanisterListPageHeading
          heading="Canister"
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
            OnGetCanisterLookup();
            CanisterAddCanisterResetStateAction();
          }}
          orderDirection={orderDirection}
          selectedOrderDirection={selectedOrderDirection}
          changeOrderDirection={(sortOrder) => {
            setSelectedOrderDirection(
              orderDirection.find((x) => x.sortOrder === sortOrder)
            );
          }}
          ExportData={() => {
            ExportCanisterData();
          }}
        />
        <CanisterModal
          modalOpen={modalOpen}
          toggleModal={() => {
            if (modalOpen) {
              setCanisterLookupData([]);
            }
            setModalOpen(!modalOpen);
            CanisterAddCanisterResetStateAction();
          }}
          canisterLookupData={canisterLookupData}
          categories={machineDropdownItem}
          onCanisterSubmit={onCanisterSubmit}
          canisterDetail={canisterDetail}
        />
        <DeleteModal
          deleteMessage={deleteMessage}
          isDeleteModalOpen={isDeleteModalOpen}
          deletedClick={() => onDeleteClick()}
          toggleModal={() => seIsDeleteModalOpen(!isDeleteModalOpen)}
        />
        <CanisterListing
          items={items}
          allCanisterItems={allCanisterItems}
          displayMode={displayMode}
          selectedItems={selectedItems}
          onCheckItem={onCheckItem}
          currentPage={currentPage}
          totalPage={totalPage}
          onContextMenuClick={onContextMenuClick}
          onContextMenu={onContextMenu}
          onChangePage={setCurrentPage}
        />
      </div>
    </>
  );
};

const mapStateToProps = ({ canister, user }) => {
  const {
    allCanisterItems,
    isSucessfullyDelete,
    isSucessfullyAdd,
    isAddError,
    canisterDetail,
    loading,
    dispenseUnitDropdownList,
    resultMessage,
    lookupData,
  } = canister;
  const { machineID } = user;
  return {
    allCanisterItems,
    isSucessfullyDelete,
    isSucessfullyAdd,
    isAddError,
    canisterDetail,
    loading,
    machineID,
    dispenseUnitDropdownList,
    resultMessage,
    lookupData,
  };
};
export default connect(mapStateToProps, {
  CanisterDeleteAction: deleteCanister,
  CanisterAddAction: addCanister,
  CanisterAddCanisterResetStateAction: addCanisterResetState,
  CanisterGetByIDAction: getByID,
  BindDispenseUnitDropdown: bindDispenseUnitDropdown,
  CanisterListAction: getCanisterList,
  DownloadCanisterExcelFileAction: downloadCanisterExcelFile,
  OnGetCanisterLookup: getCanisterLookupByCanisterCode,
})(CanisterList);
