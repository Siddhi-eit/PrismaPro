import React, { useState, useEffect } from 'react';
import useMousetrap from 'hooks/use-mousetrap';
import { getCurrentUser } from 'helpers/Utils';
import { connect } from 'react-redux';
import {
  getCanisterLookupList,
  getCanisterLookupByID,
  deleteCanisterLookup,
  addCanisterLookup,
  addCanisterLookupResetState,
  downloadCanisterLookupExcelFile,
} from 'redux/actions';
import createNotification from 'helpers/alerts';
import CanisterLookupModal from 'containers/pages/CanisterLookupModal';
import DeleteModal from 'containers/pages/DeleteModal';
import CanisterLookupListing from 'containers/canisterLookupGrid/CanisterLookupListing';
import CanisterLookupListPageHeading from 'containers/pages/CanisterLookupListPageHeading';

const orderOptions = [
  { column: '1', label: 'Canister Code' },
  { column: '2', label: 'SKU' },
  { column: '3', label: 'Name' },
  { column: '4', label: 'Created Date' },
  { column: '5', label: 'Is Active ?' },
];
const orderDirection = [
  { sortOrder: 'asc', label: 'A-Z' },
  { sortOrder: 'desc', label: 'Z-A' },
];

const pageSizes = [10, 20, 50, 100];

const CanisterLookupList = ({
  match,
  CanisterLookupListAction,
  allCanisterLookupItems,
  loading,
  machineID,
  resultMessage,
  CanisterLookupData,
  isSuccessfullyDelete,
  isSuccessfullyAdd,
  CanisterLookupSubmit,
  CanisterLookupResetState,
  CanisterLookupDeleteAction,
  CanisterLookupGetByIDAction,
  DownloadCanisterLookupExcelFileAction,
}) => {
  const [displayMode, setDisplayMode] = useState('thumblist');
  const [currentPage, setCurrentPage] = useState(1);
  const [selectedPageSize, setSelectedPageSize] = useState(10);
  const [selectedOrderOption, setSelectedOrderOption] = useState({
    column: '4',
    label: 'Created Date',
  });
  const [items, setItems] = useState([]);
  const [isLoaded, setIsLoaded] = useState(false);
  const [modalOpen, setModalOpen] = useState(false);
  const [totalItemCount, setTotalItemCount] = useState(0);
  const [totalPage, setTotalPage] = useState(1);
  const [search, setSearch] = useState('');
  const [selectedItems, setSelectedItems] = useState([]);
  const [deleteMessage, setDeleteMessage] = useState(null);
  const [isDeleteModalOpen, seIsDeleteModalOpen] = useState(false);
  const [selectedCanisterID, setSelectedCanisterID] = useState('');
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
      CanisterLookupListAction(gridObject);
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

  const startIndex = (currentPage - 1) * selectedPageSize;
  const endIndex = currentPage * selectedPageSize;

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

  const onContextMenuClick = (e, data) => {
    if (data.action === 'delete') {
      setDeleteMessage('Are you sure you want to delete?');
      seIsDeleteModalOpen(!isDeleteModalOpen);
      CanisterLookupResetState();
    } else if (data.action === 'Edit') {
      CanisterLookupGetByIDAction(selectedCanisterID);
    }
  };

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

  useEffect(() => {
    if (isSuccessfullyAdd != null && isSuccessfullyAdd > 0) {
      setModalOpen(!modalOpen);
      CanisterLookupResetState();
    }
    if (allCanisterLookupItems != null) {
      setTotalPage(allCanisterLookupItems.totalPage);
      setItems(
        allCanisterLookupItems.data.map((x) => {
          return { ...x };
        })
      );
      setSelectedItems([]);
      setTotalItemCount(allCanisterLookupItems.totalItem);
      setIsLoaded(true);
    }
  }, [
    allCanisterLookupItems,
    selectedPageSize,
    currentPage,
    selectedOrderOption,
    search,
    isSuccessfullyAdd,
    selectedOrderDirection,
  ]);

  const onDeleteClick = () => {
    seIsDeleteModalOpen(!isDeleteModalOpen);
    CanisterLookupDeleteAction(selectedCanisterID);
  };

  useEffect(() => {
    if (
      (isSuccessfullyDelete !== null &&
        isSuccessfullyDelete !== undefined &&
        isSuccessfullyDelete !== 0) ||
      (isSuccessfullyAdd != null && isSuccessfullyAdd > 0)
    ) {
      const gridObject = {
        pageSize: selectedPageSize,
        currentPage,
        orderBy: selectedOrderOption.column,
        search,
        orderDirection: selectedOrderDirection.sortOrder,
      };
      CanisterLookupListAction(gridObject);
    }
    if (isSuccessfullyAdd != null && isSuccessfullyAdd > 0) {
      createNotification('success', 'Success', resultMessage);
    }
    if (isSuccessfullyAdd != null && isSuccessfullyAdd === -1) {
      createNotification('warning', 'Warning', resultMessage);
    }
    if (isSuccessfullyAdd != null && isSuccessfullyAdd === -2) {
      createNotification('warning', 'Warning', resultMessage);
    }
    if (
      (isSuccessfullyDelete !== null || isSuccessfullyDelete !== undefined) &&
      isSuccessfullyDelete !== 0 &&
      isSuccessfullyDelete !== -1
    ) {
      createNotification('success', 'Success', 'Deleted Successfully !!');
    }
    if (isSuccessfullyDelete == -1) {
      createNotification(
        'warning',
        'Exist',
        'Code already exists in canister board'
      );
    }
  }, [isSuccessfullyDelete, isSuccessfullyAdd]);

  useEffect(() => {
    console.log('CanisterLookupData', CanisterLookupData);
    if (CanisterLookupData) {
      console.log('modalOpen', modalOpen);
      setModalOpen(!modalOpen);
      setCanisterLookupData(CanisterLookupData);
    }
  }, [CanisterLookupData]);

  const ExportCanisterLookupData = () => {
    DownloadCanisterLookupExcelFileAction();
  };

  return (
    <>
      {loading && (
        <div className="customOverlay">
          <div className="loading" />
        </div>
      )}
      <div className="disable-text-selection">
        <CanisterLookupListPageHeading
          heading="Canister Lookup"
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
            CanisterLookupResetState();
          }}
          orderDirection={orderDirection}
          selectedOrderDirection={selectedOrderDirection}
          changeOrderDirection={(sortOrder) => {
            setSelectedOrderDirection(
              orderDirection.find((x) => x.sortOrder === sortOrder)
            );
          }}
          ExportData={() => {
            ExportCanisterLookupData();
          }}
        />
        <CanisterLookupModal
          modalOpen={modalOpen}
          toggleModal={() => {
            if (modalOpen) {
              setCanisterLookupData([]);
            }
            setModalOpen(!modalOpen);
            CanisterLookupResetState();
          }}
          canisterLookupData={canisterLookupData}
          onCanisterLookupSubmit={CanisterLookupSubmit}
          canisterLookupDetail={CanisterLookupData}
        />
        <DeleteModal
          deleteMessage={deleteMessage}
          isDeleteModalOpen={isDeleteModalOpen}
          deletedClick={() => onDeleteClick()}
          toggleModal={() => seIsDeleteModalOpen(!isDeleteModalOpen)}
        />
        <CanisterLookupListing
          items={items}
          allCanisterLookupItems={allCanisterLookupItems}
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

const mapStateToProps = ({ canisterLookup, user }) => {
  const {
    allCanisterLookupItems,
    loading,
    isSuccessfullyDelete,
    isSuccessfullyAdd,
    CanisterLookupData,
    resultMessage,
  } = canisterLookup;
  const { machineID } = user;

  return {
    allCanisterLookupItems,
    isSuccessfullyDelete,
    isSuccessfullyAdd,
    CanisterLookupData,
    resultMessage,
    loading,
    machineID,
  };
};

export default connect(mapStateToProps, {
  CanisterLookupGetByIDAction: getCanisterLookupByID,
  CanisterLookupDeleteAction: deleteCanisterLookup,
  CanisterLookupListAction: getCanisterLookupList,
  CanisterLookupSubmit: addCanisterLookup,
  CanisterLookupResetState: addCanisterLookupResetState,
  DownloadCanisterLookupExcelFileAction: downloadCanisterLookupExcelFile,
})(CanisterLookupList);
